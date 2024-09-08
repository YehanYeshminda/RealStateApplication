import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { errorNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { AgreementremindersService } from '../Service/agreementreminders.service';
import { Vagree, agree } from '../agreementremider';
declare var $: any;


@Component({
  selector: 'app-agreementreminderslist',
  templateUrl: './agreementreminderslist.component.html',
  styleUrls: ['./agreementreminderslist.component.scss']
})
export class AgreementreminderslistComponent implements OnInit {
	agreereminfo$: Observable<Root<Vagree[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;

	constructor(
		private router: Router,
		private agreementremindersservices: AgreementremindersService
	) {}

	ngOnInit(): void {
		this.loadagreerem().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadagreerem(): Observable<Root<Vagree[]>> {
		return this.agreementremindersservices.getagreerem().pipe(
			tap((data) => {
				if (data.isSuccess) {
					return this.agreereminfo$ = of(data);
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
		this.router.navigateByUrl('/dashboard/agreementreminders');
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
		this.agreementremindersservices.generateReports().subscribe({
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

	navigatenew(items: Vagree){
		this.router.navigateByUrl('/dashboard/agreementreminders', { state: items });
	}

	printCellData(id: number) {
		this.agreementremindersservices.cellreport(id).subscribe({
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


