import { Component, OnInit, TemplateRef } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { Router } from '@angular/router';
import { DesignationHttpService } from './services/designation-http.service';
import { DesignationViewData } from './models/designation';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SendDynamicFormRequest } from '../staff/models/staff';
import { errorNotification, successNotification } from '../shared/notifications/notification';
import { CommonHttpService } from '../services/common-http.service';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-designation',
  templateUrl: './designation.component.html',
  styleUrls: ['./designation.component.scss']
})
export class DesignationComponent implements OnInit {
  designationinfo$: Observable<Root<DesignationViewData>> = of();
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;
  submitted2 = false;
  designationForm: FormGroup = new FormGroup({});
  get form1() { return this.designationForm.controls; }

  constructor(
      private designationHttpservices: DesignationHttpService, 
      private commonHttpService: CommonHttpService,
      private router: Router, 
      private fb: FormBuilder, 
      private modalService: NgbModal
    ) { }

  ngOnInit(): void {
    this.loaddesignation(this.page);
    this.initializeDesignationForm();
  }

  tableRefresh() {
    this.loaddesignation(1);
    this.isPaginating = true;
  }

  loaddesignation(page: number){
		this.designationHttpservices.getdesignation(page, this.pageSize).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.designationinfo$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }
    })
	}

  uploaddes(content: TemplateRef<NgbModal>): void {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }
  
  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loaddesignation(newPage);
  }

  initializeDesignationForm() {
    this.designationForm = this.fb.group({
      catergoryName: ["", [Validators.required]],
      remark: ["", [Validators.required]],
      status: [0, [Validators.required]],
    })
  }

  addNewDesignation() {
    this.submitted2 = true;
    const values: SendDynamicFormRequest = {
      authDto: GetAuthDetails(),
      dynamicField: "Designation",
      ...this.designationForm.value
    };

    this.commonHttpService.addDynamicData(values).subscribe({
      next: response => {
        if (response) {
          successNotification("Success while adding new designation");
          this.modalService.dismissAll();
          this.loaddesignation(1);
        }
      },
      error: response => {
        errorNotification("Error while adding new designation");
      }
    })
  }

}
