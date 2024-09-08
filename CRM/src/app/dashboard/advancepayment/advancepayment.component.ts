import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { getCurrentDate } from 'src/app/core/models/helpers';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { TypeComponent } from '../type/type.component';
import { environment } from 'src/environments/environment.development';
import { AdvancepaymentService } from './Service/advancepayment.service';
import { VadvPayment, advPayment } from './advpayment';

@Component({
  selector: 'app-advancepayment',
  templateUrl: './advancepayment.component.html',
  styleUrls: ['./advancepayment.component.scss']
})
export class AdvancepaymentComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  advpaymentinfo$!: VadvPayment;
  salestype$: Observable<ComboInfo[]> = of([]);
  custtype$: Observable<ComboInfo[]> = of([]);
  propreg$: Observable<ComboInfo[]> = of([]);
  accounttype$: Observable<ComboInfo[]> = of([]);
  reccheq$: Observable<ComboInfo[]> = of([]);
  isEditMode = false;
  modalRef?: BsModalRef;
  itemAdded = false;
  userImage = '';
  baseUrl = environment.apiUrl;

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private advancepaymentservices: AdvancepaymentService,
    private modalService: BsModalService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadsale();
    this.loadcustomer();
    this.getLastNo() ;
    this.loadpropreg();
    this.loadaccount();
    this.loadreccheq();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({

      id : [0],
      advno : [''],
      date :  [getCurrentDate()],
      salesby : ['', [Validators.required]],
      customer : ['', [Validators.required]],
      address  : ['', [Validators.required]],
      chequepaid : ['', [Validators.required]],
      chequeno  : ['', [Validators.required]],
      cashpaid : ['', [Validators.required]],
      cardpaid : ['', [Validators.required]],
      cardbank  : ['', [Validators.required]],
      paymentfor  : ['', [Validators.required]],
      description  : ['', [Validators.required]],
      totalpaid : [''],
    });

    const cashpaidControl = this.form.get('cashpaid');
    const chequePaidControl = this.form.get('chequepaid');
    const cardpaidControl = this.form.get('cardpaid');

    if (chequePaidControl && cardpaidControl && cashpaidControl) {
      chequePaidControl.valueChanges.subscribe(() => {
        this.calculateTotalPaid();
      });
  
      cardpaidControl.valueChanges.subscribe(() => {
        this.calculateTotalPaid();
      });

      cashpaidControl.valueChanges.subscribe(() => {
        this.calculateTotalPaid();
      });
    }

    this.advpaymentinfo$ = history.state;

    if (this.advpaymentinfo$.id) {
      this.isEditMode = true;
      this.form.patchValue({
        ...this.advpaymentinfo$,
        date: new Date(this.advpaymentinfo$.date).toISOString().split('T')[0],
      });

      this.salestype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.advpaymentinfo$.typeName);
          this.form.patchValue({
            salesby: value[valueOf].value
          })
        }
      });

      this.custtype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.advpaymentinfo$.custName);
          this.form.patchValue({
            customer: value[valueOf].value
          })
        }
      });

      this.accounttype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.advpaymentinfo$.bankCode);
          this.form.patchValue({
            cardbank: value[valueOf].value
          })
        }
      });

      this.propreg$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.advpaymentinfo$.propertname);
          this.form.patchValue({
            paymentfor: value[valueOf].value
          })
        }
      });

    } else {
      this.isEditMode = false;
    }
  }
  
  loadsale() {
    const query =
      "SELECT TypeID as _Id, TypeName as _Value FROM tblSales order by TypeID asc";
    this.salestype$ = this.commonHttpService.getComboBoxData(query);
  }

  loadcustomer() {
    const query =
      "SELECT CustID as _Id, CustName as _Value FROM tblcustomer order by CustID asc";
    this.custtype$ = this.commonHttpService.getComboBoxData(query);
  }

  loadpropreg() {
    const query =
      "SELECT id as _Id, propertname as _Value FROM tblpropertyregister order by id asc";
    this.propreg$ = this.commonHttpService.getComboBoxstring(query);
  }

  loadaccount() {
    const query =
      "SELECT ID as _Id, BankCode as _Value FROM tblcurrentacc order by ID asc";
    this.accounttype$ = this.commonHttpService.getComboBoxData(query);
  }
  
  loadreccheq() {
    const query =
      "SELECT chqid as _Id, CONCAT(chqno,'  :  ',bankid) as _Value FROM tblreccheques order by chqid asc";
    this.reccheq$ = this.commonHttpService.getComboBoxstring(query);
  }

  calculateTotalPaid() {
    const cashpaidControl = this.form.get('cashpaid');
    const chequePaidControl = this.form.get('chequepaid');
    const cardpaidControl = this.form.get('cardpaid');
    const total = this.form.get('totalpaid');
  
    if (chequePaidControl && cardpaidControl && cashpaidControl && total) {
      const chequePaid = parseFloat(chequePaidControl.value) || 0;
      const cardpaid = parseFloat(cardpaidControl.value) || 0;
      const cashpaid = parseFloat(cashpaidControl.value) || 0;
      const totalPaid = chequePaid + cardpaid + cashpaid;
      total.setValue(totalPaid.toFixed(2));
    }
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('AD', 'tblAdvPayment', 'id').subscribe({
        next: response => {
          this.form.patchValue({
            advno : response.lastValue
          })
        }
      });
    }
  }
  

  Navigateto(isSalesby: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isSalesby: isSalesby
      },
      class: 'modal-lg',
      backdrop: 'static',
    };
    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  async addEditItems() {

    this.itemAdded = true;
    try {
      if (this.advpaymentinfo$.id) {
        await this.editadvpayment();
      } else {
        await this.addadvpayment();
      }

      this.itemAdded = false;
    } catch (error) {
      console.error('Error processing item:', error);
      this.itemAdded = false;
    }

  }

  addadvpayment() {

    const data: advPayment = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.advancepaymentservices.addadvpayment(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/advancepaymentlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editadvpayment() {
    const data: advPayment = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.advpaymentinfo$.id
    }

    this.advancepaymentservices.updateadvpayment(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/advancepaymentlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


}







