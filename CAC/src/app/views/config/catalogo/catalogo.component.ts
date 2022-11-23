import { formatDate } from "@angular/common";
import { Component, Inject, OnInit, LOCALE_ID } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import { ItemModel } from "src/app/model/Util/item.model";
import { ConfigService } from "../service/config.service";

@Component({
  selector: "app-catalogo",
  templateUrl: "./catalogo.component.html",
  styleUrls: ["./catalogo.component.scss"],
})
export class CatalogoComponent implements OnInit {
  idCategoria: number = 0;
  items!: FormArray;
  name = new FormControl();
  formCatalogo!: FormGroup;
  constructor(
    private router: ActivatedRoute,
    private fb: FormBuilder,
    private serviceConfig: ConfigService,
    public dialog: MatDialog,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  ngOnInit(): void {
    this.formCatalogo = this.fb.group({
      id: 0,
      status: true,
      enable: true,
      catalogName: '',
      items: this.fb.array([]),
    });
    this.idCategoria = parseInt(
      this.router.snapshot.paramMap.get("idCatalogo") || "null"
    );
    if (this.idCategoria != 0) {
      this.getCatalogItem();
    }
  }

  setModel(model: any) {
    this.clearFormArray();
    let data: [] = model[0].items;
    data.forEach((element) => {
      (this.items = this.formCatalogo.get("items") as FormArray).push(this.addItemsArray());
    });
    this.formCatalogo.patchValue(model[0]);
  }

  addItemsArray(): FormGroup {
    return this.fb.group({
      catalogId: this.idCategoria ? 0 : this.idCategoria ,
      concept: '',
      enable: true,
      id: 0,
      itemName: '',
      status: true,
    });
  }

  clearFormArray() {
    this.formCatalogo.reset();
  }
  get item() {
    return this.formCatalogo.get("items") as FormArray;
  }

  update(element: any) {
    this.openDialogLoading(true);
    this.serviceConfig
    .updateItemWhitCatalog(element)
    .subscribe((Response) => {
      this.openDialogLoading(false);
    });
  }

  getCatalogItem() {
    this.serviceConfig
      .getItemWhitCatalog(this.idCategoria)
      .subscribe((Response) => {
        this.setModel(Response);
      });
  }

  create(element: any) {
    this.openDialogLoading(true);
    this.serviceConfig
      .addItemWhitCatalog(element)
      .subscribe((Response) => {
        this.openDialogLoading(false);
      });
  }

  add(): void {
    this.items = this.formCatalogo.get("items") as FormArray;
    this.items.push(this.addItemsArray());
  }

  remove(index: number, item : any) {
    this.openDialogLoading(true);
      this.items.removeAt(index);
      this.serviceConfig.deleteItem(item.value.id).subscribe((Response) => {
        this.openDialogLoading(false);
      },error => {
        this.openDialogLoading(false);
        console.log(error);
      });
  }

  openDialogLoading(loading: boolean): void {
    if (loading) {
      this.dialog.open(LoadingComponent, {
        //width: '300px',
        disableClose: true,
        data: {},
      });
    } else {
      this.dialog.closeAll();
    }
  }
}
