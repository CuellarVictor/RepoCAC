import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

export interface PeriodicElement {
  registros: string;
  etiqueta: string;
  asignados: string;
  variables: string;
  auditados: string;
  creado: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {etiqueta: 'Cancer mama nuevos', registros: '4500', asignados: '0', variables: '120' , auditados: '0' , creado: 'Administrador'},
  {etiqueta: 'Cancer Colon Antiguos', registros: '12000', asignados: '12000', variables: '0', auditados: '0' , creado: 'Maria Teresa'},
  {etiqueta: 'Cancer Piel Nuevos', registros: '0', asignados: '0', variables: '220', auditados: '0' , creado: 'Administrador'},
  {etiqueta: 'Cancer Piel Nuevos', registros: '0', asignados: '0', variables: '220', auditados: '0' , creado: 'Administrador'},
  {etiqueta: 'Cancer mama nuevos', registros: '4500', asignados: '0', variables: '120' , auditados: '0' , creado: 'Administrador'},
  {etiqueta: 'Cancer Colon Antiguos', registros: '12000', asignados: '12000', variables: '0', auditados: '0' , creado: 'Maria Teresa'},
];





@Component({
  selector: 'app-action-required',
  templateUrl: './action-required.component.html',
  styleUrls: ['./action-required.component.scss']
})
export class ActionRequiredComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }


  displayedColumns: string[] = ['select','luz','etiqueta', 'registros', 'asignados', 'variables', 'auditados', 'creado'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);
  selection = new SelectionModel<PeriodicElement>(true, []);
    isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: PeriodicElement): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.etiqueta + 1}`;
  }

}
