import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { LeadForwardResponse, LeadForwardViewResponse } from '../models/leadforward';
import { Observable, map, of, tap } from 'rxjs';
import { Root } from 'src/app/shared/models/baseResponse';
import { LeadsforwardService } from '../Service/leadsforward.service';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { NgxSpinnerService } from 'ngx-spinner';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { errorNotification } from 'src/app/core/models/notification';

declare var $: any;

@Component({
  selector: 'app-leadsforwardlist',
  templateUrl: './leadsforwardlist.component.html',
  styleUrls: ['./leadsforwardlist.component.scss']
})
export class LeadsforwardlistComponent implements OnInit {
  leadsForwardListInfo$: Observable<Root<LeadForwardViewResponse[]>> = of();
  selectedRowIndex: number | null = null;
  activeCount: number = 0;
  @ViewChild('rowRef', { static: false }) tRowElement!: ElementRef;
  userInfo: any;
  authCredentials: any;

  constructor(private leadsForwardHttpService: LeadsforwardService, private spinner: NgxSpinnerService, private router: Router) { }

  ngOnInit(): void {
    this.loadLeadForwards().subscribe(() => {
      this.initializeDataTable();
    });
  }

  loadLeadForwards(): Observable<Root<LeadForwardViewResponse[]>> {
    const auth: AuthDetails = getAuthDetails();
    this.spinner.show();
    return this.leadsForwardHttpService.getAllLeadForwardList().pipe(
      tap((data) => {
        if (data.isSuccess) {
          this.leadsForwardListInfo$ = of(data);
          this.spinner.hide();
        } else if (!data.isSuccess && data.message == "Invalid Hash") {
          this.router.navigateByUrl('/');
          this.spinner.hide();
        } else if (!data.isSuccess) {
          errorNotification(data.message);
          this.spinner.hide();
        }
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
    this.router.navigateByUrl('/dashboard/leadsforward');
  }

  editLeadForward(data: LeadForwardViewResponse) {
    this.router.navigateByUrl('/dashboard/leadsforward', { state: data });
  }
}
