import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonHttpService } from '../common/common-http.service';
import { Observable, of, shareReplay, tap } from 'rxjs';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { PaymentscheduleService } from './Service/paymentschedule.service';
import { PaymentSchedule, VExpenseAccountData } from './models/paymentschedule';
import { DECIMAL_REGEX, formatDate, getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { Router } from '@angular/router';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-paymentschedule',
  templateUrl: './paymentschedule.component.html',
  styleUrls: ['./paymentschedule.component.scss']
})
export class PaymentscheduleComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  supplierList$: Observable<ComboInfo[]> = of([]);
  bankList$: Observable<Root<VExpenseAccountData[]>> = of();
  editPaymentScheduleInfo: PaymentSchedule | null = null;
  editPaymentScheduleInfo$: Observable<PaymentSchedule> | null = null;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private paymentScheduleHttpService: PaymentscheduleService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadVendors();
    this.loadBanks();
    this.editPaymentScheduleInfo = history.state;
    if (this.editPaymentScheduleInfo?.PaymentScheduleNo) {
      if (this.editPaymentScheduleInfo != null) {

        this.initializeForm(this.editPaymentScheduleInfo);
      }
    } else {
      this.initializeForm();
      this.getLastNo();
    }

  }

  loadPaymentScheduleInfo(id: number): Observable<PaymentSchedule> {
    return this.paymentScheduleHttpService.getPaymentScheduleById(id, getAuthDetails()).pipe(
      tap((data) => {
        this.editPaymentScheduleInfo$ = of(data);
      })
    );
  }

  initializeForm(data?: PaymentSchedule) {
    if (data != null) {
      this.isEditMode = true;

      this.supplierList$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.SupplierName);
          this.form.patchValue({
            venderid: values[existingValue].value,
          })
        }
      })

      this.bankList$.subscribe({
        next: values => {
          const existingValue = values.result.findIndex(x => x.MainCategory == data.SupplierName);
          // this.form.patchValue({
          //   venderid: values[existingValue].value,
          // })
        }
      })

      this.form = this.fb.group({
        id: [data.PaymentScheduleNo],
        date: [formatDate(new Date(data.Date)), [Validators.required]],
        venderid: [data.SupplierName, [Validators.required]],
        reason: [data.reason, [Validators.required]],
        rxpaccount: [data.rxpaccount, [Validators.required]],
        amount: [data.amount, [Validators.required, Validators.pattern(DECIMAL_REGEX)]],
        paidon: [formatDate(new Date(data.paidon)), [Validators.required]],
        renewevery: [data.renewevery, [Validators.required]],
        renewstatus: [data.renewstatus, [Validators.required]],
        status: [data.status.toString(), [Validators.required]],
      })
    } else {
      this.isEditMode = false;
      this.form = this.fb.group({
        id: [''],
        date: [getCurrentDate()],
        venderid: ['', [Validators.required]],
        reason: ['', []],
        rxpaccount: ['', [Validators.required]],
        amount: ['', [Validators.required, Validators.pattern(DECIMAL_REGEX)]],
        paidon: [getCurrentDate()],
        renewevery: ['', [Validators.required]],
        renewstatus: ['', []],
        status: ['', [Validators.required]],
      })
    }
  }

  onSubmit() {
    const data: PaymentSchedule = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    if (this.isEditMode) {
      this.paymentScheduleHttpService.editPaymentSchedule(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.router.navigateByUrl("/dashboard/paymentschedule-list")
          } else {
            errorNotification(response.message);
          }
        }
      })
    } else {
      this.paymentScheduleHttpService.addPaymentSchedule(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            // this.form.reset();
            // this.getLastNo();
            this.router.navigateByUrl("/dashboard/paymentschedule-list")
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  }

  loadVendors() {
    const query = "SELECT SupplierID as _Id, SupplierName as _Value FROM tblsupplier WHERE status = 0 ORDER BY SupplierID DESC"
    this.supplierList$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1)
    );
  }

  loadBanks() {
    this.bankList$ = this.commonHttpService.getBankNameAndIds().pipe(
      shareReplay(1)
    );
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('PS', 'tblcontrol', 'PaymentScheduleNo').subscribe({
        next: response => {
          this.form.patchValue({
            id: response.lastValue
          })
        }
      });
    }
  }
}
