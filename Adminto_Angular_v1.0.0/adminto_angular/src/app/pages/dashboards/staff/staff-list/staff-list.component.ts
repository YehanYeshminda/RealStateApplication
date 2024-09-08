import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { Staff, VStaffListAll } from '../models/staff';
import { StaffHttpService } from '../services/staff-http.service';
import { Router } from '@angular/router';
import { confirmDeleteNotification, errorNotification, successNotification } from '../../shared/notifications/notification';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-staff-list',
  templateUrl: './staff-list.component.html',
  styleUrls: ['./staff-list.component.scss']
})
export class StaffListComponent implements OnInit {
  staffinfo$: Observable<Root<VStaffListAll>> = of();
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;
  public baseUrl = environment.signalR + "upload/staff/"

  constructor(private staffHttpService: StaffHttpService, private router: Router) { }

  ngOnInit(): void {
    this.loadstaff(this.page);
  }

  tableRefresh() {
    this.loadstaff(1);
    this.isPaginating = true;
  }

  loadstaff(page: number){
		this.staffHttpService.getstaff(page, this.pageSize).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.staffinfo$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }
    })
	}

  editStaff(staff: Staff) {
    this.router.navigateByUrl('/dashboard/staffadd', { state: staff });
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadstaff(newPage);
  }

  deleteStaff(id: number) {
		confirmDeleteNotification("Are you sure you want to delete this staff").then(response => {
			if (response.isConfirmed) {
				this.staffHttpService.deleteStaff(id).subscribe({
					next: response => {
						if (response.isSuccess) {
							successNotification(response.message);
							this.loadstaff(1);
						} else {
							errorNotification(response.message);
						}
					}
				})
			}
		})
	}
}
