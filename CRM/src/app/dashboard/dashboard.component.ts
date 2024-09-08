import { Component, HostListener, OnInit } from '@angular/core';
import { LoginUserResponse } from '../login/models/user';
import { liveNotificationResponse } from '../core/top-nav/models/notificationData';
import { environment } from 'src/environments/environment.development';
import { Router } from '@angular/router';
import { UserHttpService } from '../core/services/user-http.service';
import { NotificationHttpService } from './chart-home/service/notification-http.service';
import { errorNotification } from '../core/models/notification';
import { UserPermissionHttpService } from './user-permission/services/user-permission-http.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  username!: string; 
  designation!: string; 
  loggedIn = false;
  user!: LoginUserResponse;
  notificationCount = environment.notificationCount;
  notificationData: liveNotificationResponse[] = [];
  isToggleNav: boolean = false;
  isNotificationShow: boolean = false;
  isProfileInfoShow: boolean = false;
  clickedOncall: boolean = false;

  isCallCenterAllowed$: boolean = false;
  isLeadAllowed$: boolean = false;
  isAdvancePaymentAllowed$: boolean = false;
  isUserPermissionAllows$: boolean = false;
  isStaffAllowed$: boolean = false;
  isLeadForwardAllowed$: boolean = false;
  isLeadSegregationAllowed$: boolean = false;
  isCallInsigntAllowed$: boolean = false;
  isCallSegregationAllowed$: boolean = false;
  isCallListAllowed$: boolean = false;

  constructor(
    private router: Router,
    private userHttpService: UserHttpService,
    private notificationService: NotificationHttpService,
    private userPermissionHttpService: UserPermissionHttpService
  ) { }

  ngOnInit(): void {
    this.router.navigateByUrl("/dashboard/user-home")
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

    this.loadAllUserPermissions();
  }

  onNotificationToggle() {
    this.isNotificationShow = !this.isNotificationShow;
    this.loadAllNotifications();
  }

  onProfileClick() {
    this.isProfileInfoShow = !this.isProfileInfoShow;
    this.getusername() ;
  }

  viewportWidth: number = window.innerWidth;
  viewportHeight: number = window.innerHeight;
  isSmallToggle: boolean = false;

  @HostListener('window:resize', ['$event'])
  onResize(event: any): void {
    this.viewportWidth = event.target.innerWidth;
    this.viewportHeight = event.target.innerHeight;
  }

  
  onSideToggle() {
    this.isToggleNav = !this.isToggleNav;

    if (this.viewportWidth < 800) {
      this.isSmallToggle = !this.isSmallToggle;
    }
  }

  onSideClickToggle() {
    

    if (this.viewportWidth < 800) {
      this.isSmallToggle = !this.isSmallToggle;
      
    this.isToggleNav = !this.isToggleNav;
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

  getusername() {
    this.userPermissionHttpService.getloginuser().subscribe({
      next: response => {
        if (response.isSuccess) {
          this.username = response.result[0].name;
          this.designation = response.result[0].designation;
        } else {
          errorNotification(response.message);
        }
      },
      error: error => {
        console.error('Error:', error);
      }
    });
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

  toggleCold: boolean = false;
  toggleLead: boolean = false;

  toggleColdCalls() {
    this.toggleCold = !this.toggleCold;
  }

  toggleLeads() {
    this.toggleLead = !this.toggleLead;
  }

  onClickCall() {
    this.clickedOncall = !this.clickedOncall;
  }

  loadAllUserPermissions() {
    this.userPermissionHttpService.getAllUserPermissionsForHome().subscribe({
      next: response => {
        console.log(response)
        response.result.forEach(response => {
          if (response == "CallCenter") {
            this.isCallCenterAllowed$ = true;
          }

          if (response == "Lead") {
            this.isLeadAllowed$ = true;
          }

          if (response == "AdvancePayment") {
            this.isAdvancePaymentAllowed$ = true;
          }

          if (response == "UserPermission") {
            this.isUserPermissionAllows$ = true;
          }

          if (response == "Staffs") {
            this.isStaffAllowed$ = true;
          }

          if (response == "LeadForward") {
            this.isLeadForwardAllowed$ = true;
          }

          if (response == "LeadSegregation") {
            this.isLeadSegregationAllowed$ = true;
          }

          if (response == "CallInsight") {
            this.isCallInsigntAllowed$ = true;
          }

          if (response == "Contactlists") {
            this.isCallListAllowed$ = true;
          }

          if (response == "CallCenter") {
            this.isCallCenterAllowed$ = true;
          }
        })
      }
    })
  }
}
