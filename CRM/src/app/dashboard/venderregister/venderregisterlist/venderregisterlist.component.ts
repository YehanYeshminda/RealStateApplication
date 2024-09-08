import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { confirmNotification, errorNotification, successNotification } from 'src/app/core/models/notification';
import { venderregister } from '../venderregister';
import { VenderService } from '../Service/venderregister.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
declare var $: any;
@Component({
  selector: 'app-venderregisterlist',
  templateUrl: './venderregisterlist.component.html',
  styleUrls: ['./venderregisterlist.component.scss']
})
export class VenderregisterlistComponent implements OnInit {
	venderinfo$: Observable<Root<venderregister[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;

	constructor(
		private router: Router,
		private venderregisterservices: VenderService
	) {}

	ngOnInit(): void {
		this.loadvender().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadvender(): Observable<Root<venderregister[]>> {
		const auth: AuthDetails = getAuthDetails();

		return this.venderregisterservices.getvender(auth).pipe(
			tap((data) => {
				if (data.isSuccess) {
					return this.venderinfo$ = of(data);
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
		this.router.navigateByUrl('/dashboard/venderregister');
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
		this.venderregisterservices.generateReports().subscribe({
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

	navigatenew(items: venderregister){

		this.router.navigateByUrl('/dashboard/venderregister', { state: items });
	}

	printCellData(supplierId: number) {
		this.venderregisterservices.cellreport(supplierId).subscribe({
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

	deletevender(supplierId: number) {
		confirmNotification("Are you sure you want to delete this vender?").then(response => {
		if (response.isConfirmed) {
				this.venderregisterservices.deletevender(supplierId).subscribe({
					next: response => {
						this.router.navigateByUrl('/dashboard/venderlist');
						successNotification("vender has been deleted successfully");
						
					},
					
				});
			}
		});
	}
	
}


