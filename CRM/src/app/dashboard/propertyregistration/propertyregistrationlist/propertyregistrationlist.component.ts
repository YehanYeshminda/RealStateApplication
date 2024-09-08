import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { errorNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { PropertyregistrationService } from '../Service/propertyregistration.service';
import { PropertyRegisterView, propertyregister } from '../propertyregister';
declare var $: any;


@Component({
	selector: 'app-propertyregistrationlist',
	templateUrl: './propertyregistrationlist.component.html',
	styleUrls: ['./propertyregistrationlist.component.scss']
})
export class PropertyregistrationlistComponent implements OnInit {
	propertyregisterinfo$: Observable<Root<PropertyRegisterView[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;

	constructor(
		private router: Router,
		private propertyregistrationservices: PropertyregistrationService
	) { }

	ngOnInit(): void {
		this.loadpropertyregister().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadpropertyregister(): Observable<Root<PropertyRegisterView[]>> {
		return this.propertyregistrationservices.getpropertyregister().pipe(
			tap((data) => {
				if (data.isSuccess) {
					return this.propertyregisterinfo$ = of(data);
				} else {
					errorNotification(data.message);
					return;
				}
			}),
			catchError((error: any) => {
				if (error.status === 401) {
					this.router.navigateByUrl('/login');
				}
				return throwError(error);
			})
		);
	}

	navigate() {
		this.router.navigateByUrl('/dashboard/propertyregistration');
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
		this.propertyregistrationservices.generateReports().subscribe({
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

	navigatenew(items: PropertyRegisterView) {
		this.router.navigateByUrl('/dashboard/propertyregistration', { state: items });
	}

	printCellData(id: string) {
		this.propertyregistrationservices.cellreport(id).subscribe({
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


