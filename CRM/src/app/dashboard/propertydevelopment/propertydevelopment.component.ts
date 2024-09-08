import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { getCurrentDate } from 'src/app/core/models/helpers';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { environment } from 'src/environments/environment.development';
import { PropertyDevView, propdev } from './propdev';
import { PropertydevelopmentService } from './Service/propertydevelopment.service';

@Component({
  selector: 'app-propertydevelopment',
  templateUrl: './propertydevelopment.component.html',
  styleUrls: ['./propertydevelopment.component.scss']
})
export class PropertydevelopmentComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  propdevinfo$!: PropertyDevView;
  vendertype$: Observable<ComboInfo[]> = of([]);
  expensetype$: Observable<ComboInfo[]> = of([]);
  propregno$: Observable<ComboInfoBank[]> = of([]);
  user$: Observable<ComboInfoBank[]> = of([]);
  accounttype$: Observable<ComboInfoBank[]> = of([]);
  isEditMode = false;
  modalRef?: BsModalRef;
  itemAdded = false;
  userImage = '';
  baseUrl = environment.apiUrl;

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private propertydevelopmentservices: PropertydevelopmentService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadSupplier();
    this.loadexpense();
    this.loadpropreg();
    this.loaduser();
    this.loadaccount();
    this.initializeForm();
    this.getLastNo();
  }

  initializeForm() {
    this.form = this.fb.group({
      id: [''],
      date: [getCurrentDate()],
      propname: ['', [Validators.required]],
      vender: ['', [Validators.required]],
      propertyno: ['', [Validators.required]],
      expenseaccount: ['', [Validators.required]],
      description: ['', [Validators.required]],
      amount: ['', [Validators.required]],
      cashpaid: ['', [Validators.required]],
      banktransfer: ['', [Validators.required]],
      bankid: ['', [Validators.required]],
      chequepaid: ['', [Validators.required]],
      chequeid: ['', [Validators.required]],
      totalpaid: [''],
      approvedby: ['', [Validators.required]],
    });

    const cashpaidControl = this.form.get('cashpaid');
    const chequePaidControl = this.form.get('chequepaid');
    const bankTransferControl = this.form.get('banktransfer');

    this.form.controls['propertyno'].valueChanges.subscribe({
      next: propValue => {
        this.propregno$.subscribe({
          next: response => {
            const value = response.findIndex(x => x.value == propValue);

            this.form.patchValue({
              propname: response[value].textValue,
            })
          }
        })
      }
    })

    if (chequePaidControl && bankTransferControl && cashpaidControl) {
      chequePaidControl.valueChanges.subscribe(() => {
        this.calculateTotalPaid();
      });

      bankTransferControl.valueChanges.subscribe(() => {
        this.calculateTotalPaid();
      });

      cashpaidControl.valueChanges.subscribe(() => {
        this.calculateTotalPaid();
      });
    }

    this.propdevinfo$ = history.state;

    if (this.propdevinfo$.Id) {
      this.isEditMode = true;
      this.form.patchValue({
        ...this.propdevinfo$,
        id: this.propdevinfo$.Id,
        date: new Date(this.propdevinfo$.Date).toISOString().split('T')[0],
        expenseaccount: this.propdevinfo$.Expenseaccount,
        propertyno: this.propdevinfo$.Propertyno,
        description: this.propdevinfo$.Description,
        amount: this.propdevinfo$.Amount,
        cashpaid: this.propdevinfo$.Cashpaid,
        banktransfer: this.propdevinfo$.Banktransfer,
        chequepaid: this.propdevinfo$.Chequepaid
      });

      this.vendertype$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.propdevinfo$.SupplierName);
          this.form.patchValue({
            vender: values[existingValue].value,
          })
        }
      })

      this.user$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.propdevinfo$.ApprovedBy);
          this.form.patchValue({
            approvedby: values[existingValue].value,
          })
        }
      })
    } else {
      this.isEditMode = false;
    }
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('PD', 'tblpropdev', 'id').subscribe({
        next: response => {
          this.form.patchValue({
            id: response.lastValue
          })
        }
      });
    }
  }

  calculateTotalPaid() {
    const cashpaidControl = this.form.get('cashpaid');
    const chequePaidControl = this.form.get('chequepaid');
    const bankTransferControl = this.form.get('banktransfer');
    const total = this.form.get('totalpaid');

    if (chequePaidControl && bankTransferControl && cashpaidControl && total) {
      const chequePaid = parseFloat(chequePaidControl.value) || 0;
      const bankTransfer = parseFloat(bankTransferControl.value) || 0;
      const cashpaid = parseFloat(cashpaidControl.value) || 0;
      const totalPaid = chequePaid + bankTransfer + cashpaid;
      total.setValue(totalPaid.toFixed(2));
    }
  }

  loadpropreg() {
    const query =
      "SELECT id as _Id, propertname as _Value FROM tblpropertyregister order by id asc";
    this.propregno$ = this.commonHttpService.getComboBoxstring(query);
  }


  loadSupplier() {
    const query =
      "SELECT SupplierID as _Id, SupplierName as _Value FROM tblSupplier order by SupplierName asc";
    this.vendertype$ = this.commonHttpService.getComboBoxData(query);
  }

  loadexpense() {
    const query =
      "SELECT ID as _Id, CONCAT(MainCategory,'  :  ',SubCategory) as _Value FROM vExepnsesAccount order by ID asc";
    this.expensetype$ = this.commonHttpService.getComboBoxData(query);
  }

  loadaccount() {
    const query =
      "SELECT ID as _Id, CONCAT(code,'  :  ',BankCode) as _Value FROM tblcurrentacc order by ID asc";
    this.accounttype$ = this.commonHttpService.getComboBoxData(query);
  }

  loaduser() {
    const query =
      "SELECT userid as _Id, username as _Value FROM tblusers order by userid asc";
    this.user$ = this.commonHttpService.getComboBoxData(query);
  }

  updateCheckboxValue(controlName: string, checked: boolean) {
    const value = checked ? 1 : 0;
    this.form.get(controlName)?.setValue(value);
  }

  async addEditItems() {

    this.itemAdded = true;
    try {
      if (this.propdevinfo$.Id) {
        await this.editpropdev();
      } else {
        await this.addpropdev();
      }

      this.itemAdded = false;
    } catch (error) {
      console.error('Error processing item:', error);
      this.itemAdded = false;
    }

  }

  addpropdev() {

    const data: propdev = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.propertydevelopmentservices.addpropdev(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/propertydevelopmentlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editpropdev() {
    const data: propdev = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.propdevinfo$.Id
    }

    this.propertydevelopmentservices.updatepropdev(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/propertydevelopmentlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


}






