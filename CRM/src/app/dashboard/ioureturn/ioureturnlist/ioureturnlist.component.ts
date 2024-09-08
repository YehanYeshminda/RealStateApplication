import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { errorNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { IoureturnService } from '../Service/ioureturn.service';
import { Viourtn, iouRtn } from '../iouRtn';
declare var $: any;


@Component({
  selector: 'app-ioureturnlist',
  templateUrl: './ioureturnlist.component.html',
  styleUrls: ['./ioureturnlist.component.scss']
})
export class IoureturnlistComponent implements OnInit {
	iourtninfo$: Observable<Root<Viourtn[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;

	constructor(
		private router: Router,
		private iourtnservices: IoureturnService
	) {}

	ngOnInit(): void {
		this.loadiourtn().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadiourtn(): Observable<Root<Viourtn[]>> {
		return this.iourtnservices.getiouRtn().pipe(
			tap((data) => {
				if (data.isSuccess) {
					return this.iourtninfo$ = of(data);
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
		this.router.navigateByUrl('/dashboard/ioureturn');
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
		this.iourtnservices.generateReports().subscribe({
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

	navigatenew(items: Viourtn){
		this.router.navigateByUrl('/dashboard/ioureturn', { state: items });
	}

	printCellData(id: number) {
		this.iourtnservices.cellreport(id).subscribe({
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


