import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService , ModalOptions} from 'ngx-bootstrap/modal';
import { getCurrentDate } from 'src/app/core/models/helpers';
import { Vagree, agree } from './agreementremider';
import { AgreementremindersService } from './Service/agreementreminders.service';
import { TypeComponent } from '../type/type.component';

@Component({
  selector: 'app-agreementreminders',
  templateUrl: './agreementreminders.component.html',
  styleUrls: ['./agreementreminders.component.scss']
})
export class AgreementremindersComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  modalRef?: BsModalRef;
  agreementype$: Observable<ComboInfo[]> = of([]);
  cust$: Observable<ComboInfo[]> = of([]);
  agreereminfo$! : Vagree;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private modalService: BsModalService,
    private commonHttpService: CommonHttpService,
    private agreementremindersservices :AgreementremindersService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.loadagreement();
    this.loadcustomer();
    this.getLastNo();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      id :[0],
      agreeno : [''],
      date :[getCurrentDate()],
      custcode :['', [Validators.required]],
      agreementtype :['', [Validators.required]],
      enddate :[getCurrentDate()],
      remindon :[getCurrentDate()],
      remarks : ['', [Validators.required]],
      status :[0],
    });
  
    this.agreereminfo$ = history.state;
      
    if (this.agreereminfo$.id) {
      this.isEditMode = true;
      this.form.patchValue({
      ...this.agreereminfo$,
       date: new Date(this.agreereminfo$.date).toISOString().split('T')[0],
       remindon : new Date(this.agreereminfo$.remindon).toISOString().split('T')[0],
       enddate : new Date(this.agreereminfo$.enddate).toISOString().split('T')[0],
      });      

      this.cust$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.agreereminfo$.custName);
          this.form.patchValue({
            custcode: value[valueOf].value
          })
        }
      });

      this.agreementype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.agreereminfo$.typeName);
          this.form.patchValue({
            agreementtype: value[valueOf].value
          })
        }
      });

    } else {
      this.isEditMode = false;
    }
  }

  Navigateto(isAgree: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isAgree: isAgree
      },
      class: 'modal-lg',
      backdrop: 'static',
    };
    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }


  updateCheckboxValue(controlName: string, checked: boolean) {
		const value = checked ? 1 : 0;
		this.form.get(controlName)?.setValue(value);
	}

  loadagreement() {
    const query = "SELECT TypeID as _Id, TypeName as _Value FROM tblAgreementtype ORDER BY TypeName ASC"
    this.agreementype$ = this.commonHttpService.getComboBoxData(query);
  }

  loadcustomer(){
    const query = "SELECT CustID as _Id, CustName as _Value FROM tblcustomer ORDER BY CustName ASC"
    this.cust$ = this.commonHttpService.getComboBoxData(query);
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('AR', 'tblAgreementReminder', 'id').subscribe({
        next: response => {
          this.form.patchValue({
            agreeno : response.lastValue
          })
        }
      });
    }
  }
  

  async addEditItems() {
    
  		this.itemAdded = true;
  		try {
  			if (this.agreereminfo$.id) {
  				await this.editagreerem();
  			} else {
  				await this.addagreerem();
  			}
  			this.itemAdded = false;
  		} catch (error) {
  			console.error('Error processing item:', error);
  			this.itemAdded = false;
  		}
      
  }
  
  addagreerem() {
    const data: agree = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.agreementremindersservices.addagreerem(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/agreementreminderslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editagreerem() {
    const data: agree = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.agreereminfo$.id
    }

    this.agreementremindersservices.updateagreerem(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/agreementreminderslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  
}






