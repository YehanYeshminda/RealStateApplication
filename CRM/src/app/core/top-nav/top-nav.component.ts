import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserHttpService } from '../services/user-http.service';
import { LoginUserResponse } from 'src/app/login/models/user';
import { NotificationHttpService } from 'src/app/dashboard/chart-home/service/notification-http.service';
import { liveNotificationResponse } from './models/notificationData';
import { errorNotification } from '../models/notification';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent implements OnInit {
  loggedIn = false;
  user!: LoginUserResponse;
  notificationCount = environment.notificationCount;
  notificationData: liveNotificationResponse[] = [];

  constructor(
    private router: Router,
    private userHttpService: UserHttpService,
    private notificationService: NotificationHttpService
  ) { }


  ngOnInit(): void {
    if (sessionStorage.getItem('user')) {
      this.loggedIn = true;
      this.loadAllNotifications();
      this.loadNotificationCount();

      this.userHttpService.setUsername(
        JSON.parse(sessionStorage.getItem('user')!).username
      );

      this.userHttpService.setUserImage(
        JSON.parse(sessionStorage.getItem('user')!).userImage
      );

      this.userHttpService.setCompanyName(
        JSON.parse(sessionStorage.getItem('user')!).companyName
      );
    }
  }

  loadAllNotifications() {
    this.notificationService.getAllNotificatiosForUser().subscribe({
      next: response => {
        if (response.isSuccess) {
          this.notificationData = response.result;
        } else {
          errorNotification(response.message)
        }
      }
    })
  }

  loadNotificationCount() {
    this.notificationService.getNoficationCountForUser().subscribe({
      next: response => {
        if (response.isSuccess) {
          this.notificationCount = response.result;
        }
      }
    })
  }

  navigate() {
    if (sessionStorage.getItem('user')) {
      this.router.navigate(['/']);
      sessionStorage.removeItem('user');
    }
  }
}
