import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { SendDynamicFormRequest } from './models/dynamicForm';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { DynamicTable } from 'src/app/core/models/dynamic';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { CommonHttpServiceService } from 'src/app/core/models/common-http-service.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification, successNotification } from 'src/app/core/models/notification';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss'],
})
export class DynamicFormComponent implements OnInit, OnDestroy {
  tableData$: Observable<DynamicTable[]> = of([]);
  dynamicDropDownValues$: Observable<ComboInfo[]> = of([]);
  @Input() dynamicField = '';
  @Input() table = '';
  modalRef?: BsModalRef;
  form: FormGroup = new FormGroup({});

  constructor(
    private commonHttpService: CommonHttpServiceService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadTableData();
  }

  initializeForm() {
    this.form = this.fb.group({
      catergoryName: ["", [Validators.required]],
      remark: ["", [Validators.required]],
      status: ["", [Validators.required]],
    })
  }

  onSubmit() {
    const values: SendDynamicFormRequest = {
      authDto: getAuthDetails(),
      dynamicField: this.dynamicField,
      ...this.form.value
    }

    this.commonHttpService.addDynamicData(values).subscribe({
      next: response => {

        if (response) {
          successNotification('Successfully added!')
          this.modalRef?.hide();

          this.loadTableData();
        }
      },
    });
  }

  loadTableData() {
    if (this.table != '') {
      this.commonHttpService.getTableCommonData(this.table).subscribe(
        (data) => {
          this.tableData$ = of(data);
        },
        (error) => {
          errorNotification(error.message);
        }
      );
    }
  }

  // loadCatergory() {
  //   const query =
  //     'SELECT CategoryID as _Id, ItemCategory as _Value FROM tblItemCategory ORDER BY CategoryID DESC';
  //   this.dynamicDropDownValues$ = this.commonHttpService.getComboBoxData(query);
  // }

  ngOnDestroy(): void { }
}
