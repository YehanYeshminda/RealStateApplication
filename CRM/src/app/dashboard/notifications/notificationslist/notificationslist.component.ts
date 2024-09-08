import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { errorNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { vNofitication } from '../notifications';
import { NotificationsService } from '../Service/notifications.service';
import { NgxSpinnerService } from 'ngx-spinner';
declare var $: any;

@Component({
	selector: 'app-notificationslist',
	templateUrl: './notificationslist.component.html',
	styleUrls: ['./notificationslist.component.scss']
})
export class NotificationslistComponent implements OnInit {
	notificationinfo$: Observable<Root<vNofitication[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;

	constructor(
		private router: Router,
		private notificationsservices: NotificationsService,
		private spinner: NgxSpinnerService
	) { }

	ngOnInit(): void {
		this.loadnotification().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadnotification(): Observable<Root<vNofitication[]>> {
		this.spinner.show();
		return this.notificationsservices.getnotification().pipe(
			tap((data) => {
				if (data.isSuccess) {
					this.spinner.hide();
					return this.notificationinfo$ = of(data);
				} else {
					this.spinner.hide();
					errorNotification(data.message);
					return;
				}
			}),
			catchError((error: any) => {
				this.spinner.hide();
				if (error.status === 401) {
					this.router.navigateByUrl('/login');
				}
				return throwError(error);
			})
		);
	}

	navigate() {
		this.router.navigateByUrl('/dashboard/notifications');
	}


	initializeDataTable(): void {
		$(() => {
			const example1 = $('#example1').DataTable({
				responsive: false,
				lengthChange: false,
				autoWidth: false,
				buttons: [
					{
						extend: 'copy',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
					{
						extend: 'csv',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
					{
						extend: 'excel',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
					{
						extend: 'pdf',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
				],
			});

			example1
				.buttons()
				.container()
				.appendTo('#example1_wrapper .col-md-6:eq(0)');

			const dataTable = $('#data_table').DataTable({
				responsive: true,
				lengthChange: false,
				autoWidth: false,
				pageLength: 10,
			});

			dataTable
				.buttons([
					{
						extend: 'copy',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
					{
						extend: 'csv',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
					{
						extend: 'excel',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
					{
						extend: 'pdf',
						className: 'btn',
						style: 'background-color: #5f8286; color: white;'
					},
				])
				.container()
				.appendTo('#data_table .col-md-6:eq(0)');

			$('#data_table').off('click', '.dtr-details .input-response');
		});
	}

	printData() {
		this.notificationsservices.generateReports().subscribe({
			next: (response: HtmlResponse) => {
				if (response) {
					const printWindow = window.open('', '');

					if (printWindow) {
						printWindow.document.open();
						printWindow.document.write(response.content);
						printWindow.document.close();
						printWindow.onload = function () {
							printWindow.print();
							printWindow.close();
						};
					}
				}
			}
		});
	}

	navigatenew(items: vNofitication) {
		this.router.navigateByUrl('/dashboard/notifications', { state: items });
	}

	printCellData(id: number) {
		this.notificationsservices.cellreport(id).subscribe({
			next: (response: HtmlResponse) => {
				if (response) {
					const printWindow = window.open('', '');

					if (printWindow) {
						printWindow.document.open();
						printWindow.document.write(response.content);
						printWindow.document.close();
						printWindow.onload = function () {
							printWindow.print();
							printWindow.close();
						};
					}
				}
			}
		});
	}

}


