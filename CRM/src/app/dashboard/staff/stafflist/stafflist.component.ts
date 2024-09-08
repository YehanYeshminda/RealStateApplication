import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { confirmDeleteNotification, errorNotification, successNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { VStaffList, staff } from '../staff';
import { StaffService } from '../Service/staff.service';
declare var $: any;


@Component({
  selector: 'app-stafflist',
  templateUrl: './stafflist.component.html',
  styleUrls: ['./stafflist.component.scss']
})
export class StafflistComponent implements OnInit {
	staffinfo$: Observable<Root<VStaffList[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;

	constructor(
		private router: Router,
		private staffservices: StaffService
	) {}

	ngOnInit(): void {
		this.loadstaff().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadstaff(): Observable<Root<VStaffList[]>> {
		return this.staffservices.getstaff().pipe(
			tap((data) => {
				if (data.isSuccess) {
					return this.staffinfo$ = of(data);
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
		this.router.navigateByUrl('/dashboard/staff');
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
		this.staffservices.generateReports().subscribe({
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

	navigatenew(items: VStaffList){
		this.router.navigateByUrl('/dashboard/staff', { state: items });
	}

	printCellData(id: number) {
		this.staffservices.cellreport(id).subscribe({
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

	deleteStaff(id: number) {
		confirmDeleteNotification("Are you sure you want to delete this staff").then(response => {
			if (response.isConfirmed) {
				this.staffservices.deleteStaff(id).subscribe({
					next: response => {
						if (response.isSuccess) {
							successNotification(response.message);
							this.loadstaff().subscribe();
						} else {
							errorNotification(response.message);
						}
					}
				})
			}
		})
	}
	
}


