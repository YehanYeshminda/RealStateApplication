import { Component, OnInit } from '@angular/core';
import { NotificationNewHttpService } from '../services/notification-new-http.service';
import { Observable, of } from 'rxjs';
import { MessageResponse, MessageResponsePaginationResult, notificationdto } from './models/message';
import { Root } from 'src/app/shared/models/base';
import { MakeCenterPaginationRequest } from '../../call-center/make-calls/models/ makecall';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { Router } from '@angular/router';
import { confirmDeleteNotification, errorNotification, successNotification } from '../../shared/notifications/notification';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.scss']
})
export class NotificationListComponent implements OnInit {
  notification$: Observable<Root<MessageResponsePaginationResult>> = of();
  totalData!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;

  constructor(private notificationNewHttpService: NotificationNewHttpService, private router: Router) { }

  ngOnInit(): void {
    this.loadNotifications(this.page);
  }

  loadNotifications(page: number): void {
    this.isPaginating = true;

    const data: MakeCenterPaginationRequest = {
      authDto: GetAuthDetails(),
      page: page,
      pageSize: this.pageSize
    }

    this.notificationNewHttpService.getAllNotifications(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.notification$ = of(response);
          this.totalData = response.result.totalData;
          this.isPaginating = false;
        }
      }
    });
  }

  addNewNotification() {
    this.router.navigateByUrl("/dashboard/notificationadd")
  }

  editnotification(notification: MessageResponse) {
    this.router.navigateByUrl('/dashboard/notificationadd', { state: notification });
  }

  deletenotification(id: number) {
		confirmDeleteNotification("Are you sure you want to delete this notification").then(response => {
			if (response.isConfirmed) {
				this.notificationNewHttpService.deletenotification(id).subscribe({
					next: response => {
						if (response.isSuccess) {
							successNotification(response.message);
							this.loadNotifications(1);
						} else {
							errorNotification(response.message);
						}
					}
				})
			}
		})
	}

  tableRefresh() {
    this.loadNotifications(this.page);
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadNotifications(newPage);
  }
}
