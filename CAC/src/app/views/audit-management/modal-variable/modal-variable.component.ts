import { Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { Router } from "@angular/router";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import { VariableResponseModel } from "src/app/model/Variables/variableresponse.model";
import Swal from "sweetalert2";
import { VariableDetailsService } from "../variable-details/services/variable-details.service";

export interface DialogData {
  content: string;
  name: string;
  type: string;
}

export interface dataTable {
  check: boolean;
  name: string;
  date: string;
}

@Component({
  selector: "app-modal-variable",
  templateUrl: "./modal-variable.component.html",
  styleUrls: ["./modal-variable.component.scss"],
})
export class ModalVariableComponent {
  constructor(
    public dialogRef: MatDialogRef<ModalVariableComponent>,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private router: Router,
    private serviceVariableDetails: VariableDetailsService,
    @Inject(MAT_DIALOG_DATA) public data: VariableResponseModel
  ) {}
  mediciones: any;
  listaMediciones: any;
  formulario!: FormGroup;
  listIdMedicion: any[] = [];
  idMedicion: any;
  idCobertura: any;
  checkedItem: boolean = false;
  masterSelected: boolean = false;
  checklist: any[] = [];
  checkedList: boolean = false;
  btn: any;
  checkeada :any;
  detalleEncoded: any;
  detalleUncoded: any;
  ngOnInit(): void {
    this.mediciones = sessionStorage.getItem("Bolsas");
    this.listaMediciones = JSON.parse(this.mediciones);
    this.btn = this.mediciones = sessionStorage.getItem("boton");
    this.detalleEncoded = sessionStorage.getItem("detalle");
    this.detalleUncoded = JSON.parse(this.detalleEncoded);
    this.formulario = this.fb.group({
      idMedicion: [""],
    });
    this.checkeada =this.detalleUncoded.idMedicion;
    this.idMedicion = sessionStorage.getItem("idMedicion");
    this.idCobertura = sessionStorage.getItem("idCobertura");

    this.listaMediciones = this.listaMediciones.data.filter(
      (e: any) => e.idEnfMadre == +this.idCobertura
    );
    console.log(this.listaMediciones);
  }

  checkUncheckAll(e: any) {
    if (e.checked) {
      this.checkedList = true;
    } else {
      this.checkedList = false;
    }
  }

  eventAddOrUpdate() {
    if (this.data.variable == 0) {
      this.guardar();
    } else {
      this.actualizar();
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  addItem(i: any, e: any) {
    if (e.checked) {
      this.listIdMedicion.push(i);
    } else {
      const index: number = this.listIdMedicion.indexOf(i);
      this.listIdMedicion.splice(index, 1);
    }
  }

  actualizar() {
    this.openDialogLoading(true);
    if (this.data) {
      this.listIdMedicion.push(parseInt(this.idMedicion));
      this.data.medicionId = this.listIdMedicion;
      this.serviceVariableDetails.updateVariable(this.data).subscribe(
        (Response) => {
          this.openDialogLoading(false);
          Swal.fire({
            title: "Edición correcta",
            text: "La variable fue editada correctamente",
            icon: "success",
            confirmButtonColor: "#a94785",
            confirmButtonText: `ACEPTAR`,
          }).then(() => {
            var encoded = btoa(JSON.stringify(this.detalleUncoded));
            this.router.navigateByUrl(
              "/gestion-de-auditoria/detalle-variable/" + encoded
            );
          });
        },
        (error) => {
          this.openDialogLoading(false);
          Swal.fire({
            title: "Error",
            text: "Error en edición de variable, por favor inténta nuevamente...",
            icon: "error",
          });
        }
      );
    }
  }

  openDialogLoading(loading: boolean): void {
    if (loading) {
      this.dialog.open(LoadingComponent, {
        disableClose: true,
        data: {},
      });
    } else {
      this.dialog.closeAll();
    }
  }

  guardar() {
    this.openDialogLoading(true);
    if (this.data) {
      this.listIdMedicion.push(parseInt(this.idMedicion));
      this.data.medicionId = this.listIdMedicion;
      this.serviceVariableDetails.createVarible(this.data).subscribe(
        (Response) => {
          this.openDialogLoading(false);
          Swal.fire({
            title: "Creación Correcta.",
            text: "La variable fue guardada correctamente",
            icon: "success",
            confirmButtonColor: "#a94785",
            confirmButtonText: `ACEPTAR`,
          }).then(() => {
            var encoded = btoa(JSON.stringify(this.detalleUncoded));
            this.router.navigateByUrl(
              "/gestion-de-auditoria/detalle-variable/" + encoded
            );
          });
        },
        (error) => {
          this.openDialogLoading(false);
          Swal.fire({
            title: "Error",
            text: "Error en creación de variable, por favor inténta nuevamente...",
            icon: "error",
          });
        }
      );
    }
  }
}
