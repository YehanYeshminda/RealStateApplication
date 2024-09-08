import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Observable, map, of, tap } from 'rxjs';
import { Root } from 'src/app/shared/models/baseResponse';
import { LeadViewReponse, LeadsResponse } from '../models/leads';
import { Router } from '@angular/router';
import { LeadsService } from '../Service/leads.service';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification } from 'src/app/core/models/notification';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any;

@Component({
  selector: 'app-leadslist',
  templateUrl: './leadslist.component.html',
  styleUrls: ['./leadslist.component.scss']
})
export class LeadslistComponent implements OnInit {
  leadsListInfo$: Observable<Root<LeadViewReponse[]>> = of();
  selectedRowIndex: number | null = null;
  activeCount: number = 0;
  @ViewChild('rowRef', { static: false }) tRowElement!: ElementRef;
  userInfo: any;
  authCredentials: any;

  constructor(private router: Router, private leadsHttpService: LeadsService, private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.loadLeads().subscribe(() => {
      this.initializeDataTable();
    });
  }

  loadLeads(): Observable<Root<LeadViewReponse[]>> {
    this.spinner.show();
    return this.leadsHttpService.getAllLeads().pipe(
      tap((data) => {
        if (data.isSuccess) {
          this.leadsListInfo$ = of(data);
          this.spinner.hide();
        } else if (!data.isSuccess && data.message == "Invalid Hash") {
          this.router.navigateByUrl('/');
          this.spinner.hide();
        } else if (!data.isSuccess) {
          errorNotification(data.message);
          this.spinner.hide();
        }
      }),
      // map((data) => {
      //   const count = data.result.filter(
      //     (supplier) => supplier. === 0
      //   ).length;
      //   this.activeCount = count;
      //   return data;
      // }),
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
    this.router.navigateByUrl('/dashboard/leads');
  }

  editCrm(data: LeadViewReponse) {
    this.router.navigateByUrl('/dashboard/leads', { state: data });
  }
}
