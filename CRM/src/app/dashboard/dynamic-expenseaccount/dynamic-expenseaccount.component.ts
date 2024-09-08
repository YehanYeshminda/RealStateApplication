import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { DynamicExpenseTable, DynamicTable } from 'src/app/core/models/dynamic';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { SendDynamicexpense } from './dynamicexpenses';
import { CommonHttpService } from '../common/common-http.service';

@Component({
  selector: 'app-dynamic-expenseaccount',
  templateUrl: './dynamic-expenseaccount.component.html',
  styleUrls: ['./dynamic-expenseaccount.component.scss']
})
export class DynamicExpenseaccountComponent implements OnInit, OnDestroy {
  tableData$: Observable<DynamicExpenseTable[]> = of([]);
  maincat$: Observable<ComboInfo[]> = of([]);
  subcat$: Observable<ComboInfo[]> = of([]);
  @Input() dynamicField = '';
  @Input() table = '';
  modalRef?: BsModalRef;
  form: FormGroup = new FormGroup({});

  constructor(
    private commonHttpService: CommonHttpService, 
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadTableData();
    this.loadmainCat();
    this.loadsubCat();
  }

  initializeForm() {
    this.form = this.fb.group({
      mainCatId: ["", [Validators.required]],
      subCatId: ["", [Validators.required]],
      status: ["", [Validators.required]],
    })
  }

  onSubmit() {
    const values: SendDynamicexpense = {
      authDto: getAuthDetails(),
      dynamicField: this.dynamicField,
      ...this.form.value
    }

    this.commonHttpService.addDynamicexpense(values).subscribe({
      next: response => {

        if (response) {
          successNotification('Successfully added!')
          this.modalRef?.hide();
        }
      },
    });
  }
  loadmainCat() {
    const query =
      'SELECT ID as _Id, MainCategory as _Value FROM tblemaincat ORDER BY MainCategory asc';
    this.maincat$ = this.commonHttpService.getComboBoxData(query);
  }

  loadsubCat(){
    const query =
      'SELECT ID as _Id, SubCategory as _Value FROM tblesubcat ORDER BY SubCategory asc';
    this.subcat$ = this.commonHttpService.getComboBoxData(query);
  }

  loadTableData() {
    if (this.table != '') {
      this.commonHttpService.getdynamicexpense(this.table).subscribe(
        (data) => {
          this.tableData$ = of(data);
        },
        (error) => {
          errorNotification(error.message);
        }
      );
    }
  }



  ngOnDestroy(): void { }
}
