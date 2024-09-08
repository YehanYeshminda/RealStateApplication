import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { campaindetails } from './campaindetails';
import { CampaindetailsService } from './Service/campaindetails.service';
import { TypeComponent } from '../type/type.component';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-campaindetails',
  templateUrl: './campaindetails.component.html',
  styleUrls: ['./campaindetails.component.scss']
})
export class CampaindetailsComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  modalRef?: BsModalRef;
  issuetotype$: Observable<ComboInfo[]> = of([]);
  approvedbytype$: Observable<ComboInfo[]> = of([]);
  cust$: Observable<ComboInfo[]> = of([]);
  staff$: Observable<ComboInfo[]> = of([]);
  MediaNo$: Observable<ComboInfoBank[]> = of([]);
  selectedMediaNos: string[] = [];
  selectedNo: number = 0;
  campaindetailsinfo$! : campaindetails;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private commonHttpService: CommonHttpService,
    private modalService : BsModalService,
    private campaindetailsservices :CampaindetailsService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.loadMediaNos();
    this.initializeForm();
    this.loadstaff();
    this.loadcustomer();
    this.getLastNo();
  }

  initializeForm() {
    this.form = this.fb.group({
      no : [''],
      date :[getCurrentDate()],
      name : ['', [Validators.required]],
      datefrom :[getCurrentDate()],
      dateto :[getCurrentDate()],
      description : ['', [Validators.required]],
      totalcost : ['', [Validators.required]],
      remarks : ['', [Validators.required]],
      status : [0, [Validators.required]],
      medialink : [''],
      
    });
  
    this.campaindetailsinfo$ = history.state;
      
    if (this.campaindetailsinfo$.no) {
      this.isEditMode = true;
      this.form.patchValue({
      ...this.campaindetailsinfo$,
      
      date: new Date(this.campaindetailsinfo$.date).toISOString().split('T')[0],
      datefrom: new Date(this.campaindetailsinfo$.datefrom).toISOString().split('T')[0],
      dateto: new Date(this.campaindetailsinfo$.dateto).toISOString().split('T')[0],
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

  Navigate(isMedia: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isMedia: isMedia
      },
      class: 'modal-lg',
      backdrop: 'static',
    };

    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  selectedstaff(staffNo: string) {
    const index = this.selectedMediaNos.findIndex(x => x === staffNo);
    if (index !== -1) {
      this.selectedMediaNos.splice(index, 1);
    } else {
      this.selectedMediaNos.push(staffNo);
    }

    this.selectedNo = this.selectedMediaNos.length;
  }


  loadMediaNos() {
    this.MediaNo$ = this.campaindetailsservices.getMediaNoAndIdComboData().pipe(
      shareReplay(1)
    );
  }

  async addEditItems() {
    
  		this.itemAdded = true;
  		try {
  			if (this.campaindetailsinfo$.no) {
  				await this.editcampaindetails();
  			} else {
  				await this.addcampaindetails();
  			}

  			this.itemAdded = false;
  		} catch (error) {
  			console.error('Error processing item:', error);
  			this.itemAdded = false;
  		}
      
  }

  updateCheckboxValue(controlName: string, checked: boolean) {
		const value = checked ? 1 : 0;
		this.form.get(controlName)?.setValue(value);
	}
  
  addcampaindetails() {
    const data: campaindetails = {
      authDto: getAuthDetails(),
      mediaIds : this.selectedMediaNos,
      ...this.form.value
    }

    this.campaindetailsservices.addcampaindetails(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/campaindetailslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editcampaindetails() {
    
    const data: campaindetails = {
      authDto: getAuthDetails(),
      mediaIds : this.selectedMediaNos,
      ...this.form.value,
      id: this.campaindetailsinfo$.no
    }

    this.campaindetailsservices.updatecampaindetails(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/campaindetailslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('CA', 'tblCampainH', 'no').subscribe({
        next: response => {
          this.form.patchValue({
            no: response.lastValue
          })
        }
      });
    }
  }
  
}






