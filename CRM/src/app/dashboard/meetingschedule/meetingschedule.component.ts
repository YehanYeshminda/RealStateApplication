
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { meetsched } from './meeting';
import { MeetingscheduleService } from './Service/meetingschedule.service';
import { getCurrencySymbol } from '@angular/common';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-meetingschedule',
  templateUrl: './meetingschedule.component.html',
  styleUrls: ['./meetingschedule.component.scss']
})
export class MeetingscheduleComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  modalRef?: BsModalRef;
  issuetotype$: Observable<ComboInfo[]> = of([]);
  approvedbytype$: Observable<ComboInfo[]> = of([]);
  cust$: Observable<ComboInfo[]> = of([]);
  staff$: Observable<ComboInfo[]> = of([]);
  StaffNo$: Observable<ComboInfo[]> = of([]);
  selectedStaffNos: number[] = [];
  selectedNo: number = 0;
  meetschedinfo$! : meetsched;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private modalService: BsModalService,
    private commonHttpService: CommonHttpService,
    private meetschedservices :MeetingscheduleService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.loadStaffNos();
    this.initializeForm();
    this.loadstaff();
    this.loadcustomer();
  }

  initializeForm() {
    this.form = this.fb.group({
      id :[0],
      name :['', [Validators.required]],
      date :[getCurrentDate()],
      staffid :['', [Validators.required]],
      reason :['', [Validators.required]],
      custid :['', [Validators.required]],
      meetdate :[getCurrentDate()],
      meettime :['', [Validators.required]],
      venue :['', [Validators.required]],
      remarks :['', [Validators.required]],
      status :[0, [Validators.required]],
      conclusion:['Not Updated']
    });
  
    this.meetschedinfo$ = history.state;
      
    if (this.meetschedinfo$.id) {
      this.isEditMode = true;
      this.form.patchValue({
      ...this.meetschedinfo$,
       date: new Date(this.meetschedinfo$.date).toISOString().split('T')[0],
       meetdate: new Date(this.meetschedinfo$.meetdate).toISOString().split('T')[0],
    });
    } else {
      this.isEditMode = false;
    }
  }

  loadstaff() {
    const query = "SELECT id as _Id, name as _Value FROM tblstaffs ORDER BY name ASC"
    this.staff$ = this.commonHttpService.getComboBoxData(query);
  }

  loadcustomer(){
    const query = "SELECT CustID as _Id, CustName as _Value FROM tblcustomer ORDER BY CustName ASC"
    this.cust$ = this.commonHttpService.getComboBoxData(query);
  }

  selectedstaff(id: number) {
    const index = this.selectedStaffNos.findIndex(x => x === id);
    if (index !== -1) {
      this.selectedStaffNos.splice(index, 1);
    } else {
      this.selectedStaffNos.push(id);
    }
    this.selectedNo = this.selectedStaffNos.length;
  }

  loadStaffNos() {
    this.StaffNo$ = this.meetschedservices.getStaffNoAndIdComboData().pipe(
      shareReplay(1)
    );
  }

  async addEditItems() {
    
  		this.itemAdded = true;
  		try {
  			if (this.meetschedinfo$.id) {
  				await this.editmeetsched();
  			} else {
  				await this.addmeetsched();
  			}

  			this.itemAdded = false;
  		} catch (error) {
  			console.error('Error processing item:', error);
  			this.itemAdded = false;
  		}
      
  }
  
  addmeetsched() {
    const meettimeControl = this.form.get('meettime');
    const meettimes = meettimeControl ? meettimeControl.value : '';

    const data: meetsched = {
      authDto: getAuthDetails(),
      meettime : meettimes,
      staffIds : this.selectedStaffNos,
      ...this.form.value
    }

    this.meetschedservices.addmeetsched(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/meetschedlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editmeetsched() {
    const meettimeControl = this.form.get('meettime');
    const meettimes = meettimeControl ? meettimeControl.value : '';
    const data: meetsched = {
      authDto: getAuthDetails(),
      meettime : meettimes,
      staffIds : this.selectedStaffNos,
      ...this.form.value,
      id: this.meetschedinfo$.id
    }

    this.meetschedservices.updatemeetsched(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/meetingschedulelist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  
}






