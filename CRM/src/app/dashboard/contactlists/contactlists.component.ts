import { Component, OnInit } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { errorNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { ContactlistService } from './Service/contactlist.service';
import { calllist } from './contactlist';
declare var $: any;

@Component({
  selector: 'app-contactlists',
  templateUrl: './contactlists.component.html',
  styleUrls: ['./contactlists.component.scss']
})
export class ContactlistsComponent implements OnInit {
	calllistinfo$: Observable<Root<calllist[]>> = of();
	selectedRowIndex: number | null = null;
	activeCount: number = 0;
	selectedFile: File | null = null;

	constructor(
		private router: Router,
		private contactlistservices: ContactlistService
	) {}

	ngOnInit(): void {
		this.loadcalllist().subscribe(() => {
			this.initializeDataTable();
		});
	}

	loadcalllist(): Observable<Root<calllist[]>> {
		return this.contactlistservices.getcalllist().pipe(
			tap((data) => {
				if (data.isSuccess) {
					return this.calllistinfo$ = of(data);
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

	onFileSelected(event: any) {
		this.selectedFile = event.target.files[0];
	}
	
	onUpload() {
		if (this.selectedFile) {
		  this.contactlistservices.uploadExcelFile(this.selectedFile).subscribe(
			response => {
			  if (response.isSuccess) {
				alert('File uploaded successfully');
			  } else {
				alert('Error uploading file: ' + response.message);
			  }
			},
			error => {
			  console.error('Error uploading file: ', error);
			}
		  );
		} else {
		  alert('Please select a file to upload.');
		}
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
	

	
}


