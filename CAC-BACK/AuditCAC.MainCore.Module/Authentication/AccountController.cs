using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
//using System.DirectoryServices;
using System.DirectoryServices;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Authentication
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase 
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly IMapper _mapper;
        private readonly IRepository<CurrentProcessModel> _repository;
        private readonly ILogger<RegistrosAuditoriaDetalleModel> _logger;
        
        public string connectionString { get; set; }

        //Constructor del controller.
        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, 
            SignInManager<IdentityUser> signInManager, 
            DBAuditCACContext dBAuditCACContext,
            IMapper mapper,
            IRepository<CurrentProcessModel> repository,
            ILogger<RegistrosAuditoriaDetalleModel> logger
            //AspNetUserManager<TUser> user
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            //_user = user; 
            this.configuration = configuration;
            this.signInManager = signInManager;
            this._dBAuditCACContext = dBAuditCACContext;
            _mapper = mapper;
            _repository = repository;
            _logger = logger;

            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        //Creamos un usuario con Identity.
        /// <summary>
        /// Crear un nuevo usuario.
        /// </summary>
        /// <remarks>
        /// Permite crear un nuevo usuario.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost("create")]
        public async Task<ActionResult<ResponseAuthenticationDto>> CreateOrUpdate(UserCredentialsCreateDto userCredentialsDto)
          {
            try
             {
                //Creamos usuario, pasando el IdentityUser creado previamente.
                var Result = new IdentityResult();
        
                if (!String.IsNullOrEmpty(userCredentialsDto.Id)) //Edicion de usuario
                { 
                    //Creamos IdentityUser.
                    var User = new IdentityUser
                    {
                        Id = userCredentialsDto.Id,
                        UserName = userCredentialsDto.Email,
                        Email = userCredentialsDto.Email,
                        PhoneNumber = userCredentialsDto.PhoneNumber, 
                    };

                    //consultamos el usuario actual en el contexto
                    var updatedUser = await userManager.FindByIdAsync(User.Id);
                 
                    updatedUser.UserName = User.Email;
                    updatedUser.Email = User.Email;
                    updatedUser.PhoneNumber = User.PhoneNumber;

                    Result = await userManager.UpdateAsync(updatedUser);

                    //Validamos si el usuario fue creado.
                    if (Result.Succeeded)
                    {
                        //Consultamos usuario creado.
                        var AspNetUsers = _dBAuditCACContext.AspNetUsersDetellesModel.FirstOrDefault(x => x.AspNetUsersId == User.Id);

                        if (AspNetUsers != null)
                        {
                            //Editamos Usuario, agregando campos pendientes.    
                            AspNetUsers.Codigo = userCredentialsDto.Code;
                            AspNetUsers.Nombres = userCredentialsDto.Name;
                            AspNetUsers.Apellidos = userCredentialsDto.LastName;

                            await _dBAuditCACContext.SaveChangesAsync();
                        }
                        else
                        {
                            AspNetUsersDetellesModel AspNetUsersCreate = new AspNetUsersDetellesModel();

                            AspNetUsersCreate.Id = 0;
                            AspNetUsersCreate.AspNetUsersId = User.Id;
                            AspNetUsersCreate.Codigo = userCredentialsDto.Code;
                            AspNetUsersCreate.Nombres = userCredentialsDto.Name;
                            AspNetUsersCreate.Apellidos = userCredentialsDto.LastName;

                            _dBAuditCACContext.AspNetUsersDetellesModel.Add(AspNetUsersCreate);
                            await _dBAuditCACContext.SaveChangesAsync(); 
                        }  

                        //Actualizamos Rol.                    
                        var Rol = roleManager.Roles.Where(x => x.Id == userCredentialsDto.RolId.ToString()).FirstOrDefault(); 
                        var oldRoleName = roleManager.Roles.Where(x => x.Id == userCredentialsDto.oldRolId.ToString()).FirstOrDefault(); 

                        if (Rol != null)
                        {
                           var DeleteRol = await userManager.RemoveFromRoleAsync(updatedUser, oldRoleName.NormalizedName);
                           var ResultRol = await userManager.AddToRoleAsync(updatedUser, Rol.NormalizedName);
                        }

                        //Enlazamos el usuario actualizado a las enfermedades
                        var usuarioEnfermedad = updatedUser.Id; 

                        //Consultamos las enfermedades a borrar                
                        var oldUxE = _dBAuditCACContext.GetFiltrosBolsaMedicion.Where(x => x.IdUsuario == usuarioEnfermedad.ToString()).ToList();

                        //Consultamos el createdby para el registro.                    
                        var oldUxEU = _dBAuditCACContext.GetFiltrosBolsaMedicion.Where(x => x.IdUsuario == usuarioEnfermedad.ToString()).FirstOrDefault();

                        foreach (var old in oldUxE){
                            var resultDeleteUxE = _dBAuditCACContext.GetFiltrosBolsaMedicion.Remove(old);
                        }

                        if(oldUxEU != null)
                        {
                            foreach (var e in userCredentialsDto.Enfermedades)
                            {
                                var UxE = new UsuarioXEnfermedadModel();

                                UxE.IdUsuario = usuarioEnfermedad;
                                UxE.IdCobertura = e;
                                UxE.Activo = true;
                                UxE.CreatedBy = oldUxEU.CreatedBy;
                                UxE.CreatedDate = oldUxEU.CreatedDate;
                                UxE.ModifyBy = userCredentialsDto.ModifyBy;
                                UxE.ModifyDate = DateTime.Now;

                                var resUxE = _dBAuditCACContext.GetFiltrosBolsaMedicion.Add(UxE);
                            }
                        }
                        else
                        {
                            foreach (var e in userCredentialsDto.Enfermedades)
                            {
                                var UxE = new UsuarioXEnfermedadModel();

                                UxE.IdUsuario = usuarioEnfermedad;
                                UxE.IdCobertura = e;
                                UxE.Activo = true;
                                UxE.CreatedBy = userCredentialsDto.ModifyBy;
                                UxE.CreatedDate = DateTime.Now;
                                UxE.ModifyBy = userCredentialsDto.ModifyBy;
                                UxE.ModifyDate = DateTime.Now;

                                var resUxE = _dBAuditCACContext.GetFiltrosBolsaMedicion.Add(UxE);
                            }
                        }

                        await _dBAuditCACContext.SaveChangesAsync();
                    }
                }
                else //Creacion de usuario
                {
                    //Creamos IdentityUser.
                    var User = new IdentityUser
                    { 
                        UserName = userCredentialsDto.UserName,
                        Email = userCredentialsDto.Email,
                        PhoneNumber = userCredentialsDto.PhoneNumber,
                    };

                    Result = await userManager.CreateAsync(User, userCredentialsDto.Password);

                    //Validamos si el usuario fue creado.
                    AspNetUsersDetellesModel AspNetUsersCreate = new AspNetUsersDetellesModel();

                    if (Result.Succeeded)
                    {
                        AspNetUsersCreate.Id = 0;
                        AspNetUsersCreate.AspNetUsersId = User.Id;
                        AspNetUsersCreate.Codigo = userCredentialsDto.Code;
                        AspNetUsersCreate.Nombres = userCredentialsDto.Name;
                        AspNetUsersCreate.Apellidos = userCredentialsDto.LastName;

                        _dBAuditCACContext.AspNetUsersDetellesModel.Add(AspNetUsersCreate);
                    }

                    //Creamos Rol.                    
                    var Rol = roleManager.Roles.Where(x => x.Id == userCredentialsDto.RolId.ToString()).FirstOrDefault();
                    if (Rol != null)
                    {
                        var ResultRol = await userManager.AddToRoleAsync(User, Rol.NormalizedName);
                    }

                    //Enlazamos el usuario creado a las enfermedades
                    var usuarioEnfermedad = AspNetUsersCreate.AspNetUsersId;

                    foreach(var e in userCredentialsDto.Enfermedades)
                    {
                        var UxE = new UsuarioXEnfermedadModel();

                        UxE.Id = 0;
                        UxE.IdUsuario = usuarioEnfermedad;
                        UxE.IdCobertura = e;
                        UxE.Activo = true;
                        UxE.CreatedBy = userCredentialsDto.CreatedBy;
                        UxE.CreatedDate = DateTime.Now; 
                        UxE.ModifyDate = DateTime.Now;

                        var resUxE = _dBAuditCACContext.GetFiltrosBolsaMedicion.AddAsync(UxE);
                    } 

                    await _dBAuditCACContext.SaveChangesAsync();
                }

                //Validamos si el usuario fue creado.
                if (Result.Succeeded)
                {

                    UserCredentialsDto userCredentialsDtoTK = new UserCredentialsDto();
                    userCredentialsDtoTK.Email = userCredentialsDto.Email;
                    userCredentialsDtoTK.Password = userCredentialsDtoTK.Password;

                    return new ResponseAuthenticationDto();// await BuildToken(userCredentialsDtoTK);
                }
                else
                {
                    //En caso de no ser creado, retornamos un BadRequest con los errores.
                    return BadRequest(Result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return BadRequest("Error "+ ex.ToString());
            }
        }

        ////Creamos un usuario con Identity.
        ///// <summary>
        ///// Crear un nuevo usuario.
        ///// </summary>
        ///// <remarks>
        ///// Permite crear un nuevo usuario.
        ///// </remarks>
        ///// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        ///// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        ///// <response code="500">Error general, informar al administrador del servicio.</response>
        ///// <returns>Configruacion ADO</returns>
        //[HttpPost("create")]
        //public async Task<ActionResult<ResponseAuthenticationDto>> Create(UserCredentialsDto userCredentialsDto)
        //{
        //    //Creamos IdentityUser.
        //    var User = new IdentityUser { UserName = userCredentialsDto.Email, Email = userCredentialsDto.Email };

        //    //Creamos usuario, pasando el IdentityUser creado previamente.
        //    var Result = await userManager.CreateAsync(User, userCredentialsDto.Password);

        //    //Validamos si el usuario fue creado.
        //    if (Result.Succeeded)
        //    {
        //        return BuildToken(userCredentialsDto);
        //    }
        //    else
        //    {
        //        //En caso de no ser creado, retornamos un BadRequest con los errores.
        //        return BadRequest(Result.Errors);
        //    }
        //}

        //Iniciamos sesion, dependiendo el caso, se puede iniciar con Identity o con Directorio Activo.
        /// <summary>
        /// Iniciar sesión.
        /// </summary>
        /// <remarks>
        /// Permite iniciar sesión, para acceder a módulos privados. Se puede acceder con un usuario registrado en el sistema o con un usuario registrado en el ActiveDirectory.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ResponseAuthenticationDto>> Login(UserCredentialsDto userCredentialsDto)
        {
            try
            {
                /* 
                 * 1. Enviar un parametro tipo Bool, true es para Directorio activo, false es para Identity. Se valida el campo y se usa la autenticacion requerida/
                 * 2. Validar primero si el usuario existe en Directorio Activo, en ese caso... ingresar normal y agregar el tipo de logueo, en este caso directorio activo
                 * 3. Validar primero si el usuario existe en Database (Identity), en ese caso... ingresar normal y agregar el tipo de logueo, en este caso Identity.
                 * 4. Se consultan ambas cosas al tiempo (Identity y Directorio activo). Luego se valida si el usuario existe en alguna de las dos opciones. En caso de existir, se construye el Token, con la info del usuario. En caso contrario se da un BadRequest.
                 * Se plantea usar de momento, la opción numero 4.
                 */

                //Buscamos el usuario, con su contraseña en la base de datos. Nota: Por defecto se desactiva bloqueo de inicio de sesión fallidos.
                //var ResultIdentity = await signInManager.PasswordSignInAsync(userCredentialsDto.Email, userCredentialsDto.Password, isPersistent: false, lockoutOnFailure: false);

                var auth = await _repository.Login(userCredentialsDto);

                //Validamos si esta activada la autenticación por AD.
                var ResultAD = false;

                #region Directorio Activo

                var ActiveAirectoryAuthenticationIsActivate = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == 10).Where(x => x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (ActiveAirectoryAuthenticationIsActivate == null || ActiveAirectoryAuthenticationIsActivate == "") { ActiveAirectoryAuthenticationIsActivate = "0"; } //Valor por defecto si no encuentra parametro.
                if (ActiveAirectoryAuthenticationIsActivate == "1")
                {
                    //Buscamos usuario en Directorio Activo.
                    ResultAD = AuthenticateAD(userCredentialsDto.Email, userCredentialsDto.Password);
                }

                #endregion


                //validamos si el usuario existe.
                return await BuildToken(auth);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return BadRequest("Error. Puede que el sistema no este disponible.");
            }
        }


        //Metodo para autenticar mediante Directorio Activo.
        private bool AuthenticateAD(string UserName, string Password) //string
        {
            try
            {
                //Declaramos variable por defecto en False.
                var UserNameAD = "";
                bool Valid = false;

                //Obtenemos AD actual, enviando UserName y Password del usuario.
                var DomainName = "AutenticationTestAD.local";
                DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + DomainName, UserName, Password);
                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                SearchResult Results = null;

                //Obtenemos resultado
                Results = directorySearcher.FindOne();
                Valid = true;         

                //Retornamos valor.
                return Valid;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return false;
            }
        }

        //Metodo para obtener usuario mediante Directorio Activo.
        private bool GetUserAD(string UserName, string Password) //string
        {
            try
            {
                //Variables.
                var Names = "";
                var Valid = false;

                //Obtenemos datos del AD.
                SearchResultCollection Results;
                DirectorySearcher directorySearcher = null;
                DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://AutenticationTestAD.local");
                //DirectoryEntry directoryEntry = new DirectoryEntry(GetCurrentDomainPath());

                //Consultamos usuarios en el AD obtenido.
                directorySearcher = new DirectorySearcher(directoryEntry);
                directorySearcher.Filter = "(&(objectCategory=User)(objectClass=person))";
                directorySearcher.Filter = "(&(objectCategory=User)(objectClass=person)(samaccountname=" + UserName + "*))";

                //Buscamos el/los usuarios del AD que corresponda.
                Results = directorySearcher.FindAll();

                //Recorremos el/los usuarios del AD que corresponda.
                foreach (SearchResult sr in Results)
                {
                    var UserNameAD = sr.Properties["samaccountname"][0].ToString(); //11

                    //Validamos si el UserName recibido es igual al UserName registrado en AD. (Esto es solo una prueba, no debe ser usado ya que es muy lento, debe recorrer todos los Nombres en el DA) SOLO PRUEBA.
                    if (UserName.Contains(UserNameAD))
                    {
                        Valid = true;
                        break;
                    }
                }

                //Retornamos true si encontramos el usuario.
                return Valid;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return false;
            }
        }

        //Metodo para construir Token de usuario.
        private async Task<ResponseAuthenticationDto> BuildToken(ResponseAutenticacionDto input)
        {

            #region Tiempo de expiracion
                //Obtenemos y calculamos Tiempos de Expiración y de Inactividad.                      
                var InactivityTime = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == 7).Where(x => x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (InactivityTime == null || InactivityTime == "")
                {
                    InactivityTime = "5"; //Valor por defecto. Configurado en Minutos. 
                }

                var ExpirationTime = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == 8).Where(x => x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (ExpirationTime == null || ExpirationTime == "")
                {
                    ExpirationTime = "60"; //Valor por defecto. Configurado en Minutos.
                }

                var SessionDead = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == 9).Where(x => x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (SessionDead == null || SessionDead == "")
                {
                    SessionDead = "1"; //Valor por defecto. Configurado en Minutos.
                }
            #endregion


           //Agregamos Claims del usuario (Email). Tiempo de inactividad y Tiempo de expiración del Token.
            var Claims = new List<Claim>()
            {
                new Claim("Email", input.Usuario),
                new Claim("InactividadTime", InactivityTime.ToString()),
                //new Claim("ExpirationTime", ExpirationTime.ToString())
            };

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            bool isAdmin = currentUser.IsInRole("Admin");
            var test = currentUser.Identity.Name;

            //Firmamos el Jwt (La Secret key debe guardarse en un mejor lugar).
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            //Definimos expiracion del Token (El valor en tiempo, debe ser definido en un parametro).
            var Expiration = DateTime.Now.AddMinutes(int.Parse(ExpirationTime)); //15

            //Construimos Token.
            var SecurityToken = new JwtSecurityToken(issuer: null, audience: null, claims: Claims, expires: Expiration, signingCredentials: Credentials);

            List<PermisoRol> permisos = new List<PermisoRol>();

            if(input.Id == (int)Enumeration.RespuestaLogin.Exitoso)
            {
                permisos = await _repository.ConsultaPermisosRol(input.RolId);
                permisos = permisos.Where(x => x.Visible).ToList();
            }
            else if (input.Id == (int)Enumeration.RespuestaLogin.UsuarioBloqueadoPorIntentosFallidos)
            {
                _repository.EnviarCorreoRecuperacionPassword(input);
            }

            //Build Response
            var Respuesta = new ResponseAuthenticationDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken),
                Expiration = Expiration,
                InactivityTime = int.Parse(InactivityTime),
                //ExpirationTime = int.Parse(ExpirationTime),
                SessionDead = int.Parse(ExpirationTime),
                //
                objUsuario = new objUser() {
                    UserId = input.UserId,
                    UserName = input.Usuario,
                    Rol = new objRol() {
                        UserRolName = input.NombreRol,
                        UserRolId = input.RolId
                    },
                    Name = input.Nombres,
                    Codigo = input.Codigo
                },
                MensajeRespuesta = input.MensajeRespuesta,
                CodigoRespuesta = input.Id,
                PermisoRol = permisos,
                EsLider = (bool)input.EsLider
            };

            //Retornamos.
            return Respuesta;
        }


        /// <summary>
        /// Valida token recuperacion de contraseña
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>Mensaje</returns>
        [HttpGet("ValidaTokenRecuperacion/{token}")]
        public async Task<ResponseLlaveValor> ValidaTokenRecuperacion(string token)
        {
            try
            {
                var auth = await _repository.ValidaTokenRecuperacion(token);
                return auth;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Actualiza password, proceso recuperación contraseña
        /// </summary>
        /// <param name="input">Modelo: token, password</param>
        [HttpPost]
        [Route("ActualizaPassword")]
        public async Task<ResponseLlaveValor> ActualizaPassword(ActualizaPassword input)
        {
            try
            {
                var result = await _repository.ActualizaPassword(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Recuperar contraseña
        /// </summary>
        /// <param name="input">Modelo usuario</param>
        /// <returns>Mensaje Respuesta</returns>
        [HttpPost]
        [Route("RecuperarPassword")]
        public async Task<ResponseLlaveValor> RecuperarPassword(UserCredentialsDto input)
        {
            try
            {
                var result = await _repository.RecuperarPassword(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        // <summary>
        /// Cierra sesion activa
        /// </summary>
        /// <param name="userId">Usuario</param>
        [HttpGet("CerrarSesion/{userId}")]
        public async Task<bool> CerrarSesion(string userId)
        {
            try
            {
                await _repository.CerrarSesion(userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        [HttpPost("ExecuteScript")]
        public async Task<IActionResult> ExecuteScript([FromBody] ResponseLlaveValor input)
        {
            try
            {
                var Data = await _repository.ExecuteScript(connectionString, input.Valor);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }



        [HttpPost("loadlogs3")]
        public async Task<IActionResult> loadlogs3([FromBody] TestLoadLog input)
        {
            try
            {
                S3Model s3 = new S3Model()
                {
                    S3AccessKeyId = configuration["S3:S3AccessKeyId"],
                    S3SecretAccessKey = configuration["S3:S3SecretAccessKey"],
                    S3RegionEndpoint = configuration["S3:S3RegionEndpoint"],
                    S3BucketName = configuration["S3:S3BucketName"],
                    S3StorageClass = configuration["S3:S3StorageClass"],
                    UrlFileS3Uploaded = configuration["S3:UrlFileS3Uploaded"]
                };
                var Data = await _repository.loadlogs3(input.FilePath.Replace("/", "\\"), input.FileName, s3);


                return Ok(configuration["S3:UrlFileS3Uploaded"] + input.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }
    }
}
