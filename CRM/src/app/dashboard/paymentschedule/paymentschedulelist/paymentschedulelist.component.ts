import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, map, of, tap } from 'rxjs';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { PaymentScheduleList } from '../models/paymentschedule';
import { Root } from 'src/app/shared/models/baseResponse';
import { PaymentscheduleService } from '../Service/paymentschedule.service';
import { errorNotification } from 'src/app/core/models/notification';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any;

@Component({
  selector: 'app-paymentschedulelist',
  templateUrl: './paymentschedulelist.component.html',
  styleUrls: ['./paymentschedulelist.component.scss']
})
export class PaymentschedulelistComponent {
  paymentScheduleInfo$: Observable<Root<PaymentScheduleList[]>> = of();
  selectedRowIndex: number | null = null;
  activeCount: number = 0;
  @ViewChild('rowRef', { static: false }) tRowElement!: ElementRef;
  userInfo: any;
  authCredentials: any;

  constructor(private router: Router, private paymentScheduleHttpService: PaymentscheduleService, private spinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
    this.loadPaymentSchedules().subscribe(() => {
      this.initializeDataTable();
    });
  }

  loadPaymentSchedules(): Observable<Root<PaymentScheduleList[]>> {
    const auth: AuthDetails = getAuthDetails();
    this.spinnerService.show();

    return this.paymentScheduleHttpService.getAllPaymentSchedule(auth).pipe(
      tap((data) => {
        if (data.isSuccess) {
          this.paymentScheduleInfo$ = of(data);
          this.spinnerService.hide();
        } else if (!data.isSuccess && data.message == "Invalid Hash") {
          this.spinnerService.hide();
          this.router.navigateByUrl('/');
        } else if (!data.isSuccess) {
          this.spinnerService.hide();
          errorNotification(data.message);
        }
      }),
      map((data) => {
        const count = data.result.filter(
          (supplier) => supplier.status === 0
        ).length;
        this.activeCount = count;
        return data;
      }),
    );
  }

  initializeDataTable(): void {
    $(() => {
      const example1 = $('#example1').DataTable({
        responsive: false,
        lengthChange: false,
        autoWidth: true,
        pageLength: 10,
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print', 'colvis'],
      });

      example1
        .buttons()
        .container()
        .appendTo('#example1_wrapper .col-md-6:eq(0)');

      const dataTable = $('#data_table').DataTable({
        responsive: false,
        lengthChange: false,
        autoWidth: true,
        pageLength: 5,
      });

      dataTable
        .buttons(['copy', 'csv', 'excel', 'pdf', 'print', 'colvis'])
        .container()
        .appendTo('#data_table .col-md-6:eq(0)');

      $('#data_table')
        .off('click', '.input-response')
        .on('click', '.input-response', (event: any) => {
          // this.getTbodyValue();
        });

      $('#data_table').off('click', '.dtr-details .input-response');
    });
  }

  navigate() {
    this.router.navigateByUrl('/dashboard/paymentschedule');
  }

  editGrnH(data: PaymentScheduleList) {
    this.router.navigateByUrl('/dashboard/paymentschedule', { state: data });
  }
}
