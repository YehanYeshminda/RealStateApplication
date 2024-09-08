import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { errorNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { AdvancepaymentService } from '../Service/advancepayment.service';
import { VadvPayment, advPayment } from '../advpayment';
declare var $: any;


@Component({
  selector: 'app-advancepaymentlist',
  templateUrl: './advancepaymentlist.component.html',
  styleUrls: ['./advancepaymentlist.component.scss']
})
export class AdvancepaymentlistComponent implements OnInit {
	advpaymentinfo$: Observable<Root<VadvPayment[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;

	constructor(
		private router: Router,
		private advancepaymentservices: AdvancepaymentService
	) {}

	ngOnInit(): void {
		this.loadadvpayment().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadadvpayment(): Observable<Root<VadvPayment[]>> {
		return this.advancepaymentservices.getadvpayment().pipe(
			tap((data) => {
				if (data.isSuccess) {
					return this.advpaymentinfo$ = of(data);
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
		this.router.navigateByUrl('/dashboard/advancepayment');
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
				className: 'btn btn-primary',
			  },
			  {
				extend: 'csv',
				className: 'btn btn-primary',
			  },
			  {
				extend: 'excel',
				className: 'btn btn-primary',
			  },
			  {
				extend: 'pdf',
				className: 'btn btn-primary',
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
				className: 'btn btn-primary',
			  },
			  {
				extend: 'csv',
				className: 'btn btn-primary',
			  },
			  {
				extend: 'excel',
				className: 'btn btn-primary',
			  },
			  {
				extend: 'pdf',
				className: 'btn btn-primary',
			  },
			])
			.container()
			.appendTo('#data_table .col-md-6:eq(0)');
	  
		  $('#data_table').off('click', '.dtr-details .input-response');
		});
	}
	
	printData() {
		this.advancepaymentservices.generateReports().subscribe({
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

	navigatenew(items: VadvPayment){
		this.router.navigateByUrl('/dashboard/advancepayment', { state: items });
	}

	printCellData(id: number) {
		this.advancepaymentservices.cellreport(id).subscribe({
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


