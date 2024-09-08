import { Component, OnInit } from '@angular/core';
import { UserPermissionHttpService } from '../user-permission/services/user-permission-http.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(private userPermissionHttpService: UserPermissionHttpService) {}

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

  ngOnInit(): void {
    this.loadAllUserPermissions();
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
