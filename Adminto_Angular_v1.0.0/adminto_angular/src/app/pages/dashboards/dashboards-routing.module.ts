import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardOneComponent } from './dashboard-one/dashboard-one.component';
import { LeadsComponent } from './leads/leads.component';
import { LeadAddEditComponent } from './leads/lead-add-edit/lead-add-edit.component';
import { LeadForwardComponent } from './lead-forward/lead-forward.component';
import { LeadForwardAddEditComponent } from './lead-forward/lead-forward-add-edit/lead-forward-add-edit.component';
import { LeadSegregationComponent } from './lead-segregation/lead-segregation.component';
import { MakeCallsComponent } from './call-center/make-calls/make-calls.component';
import { CallListComponent } from './call-center/call-list/call-list.component';
import { CallSegregationComponent } from './call-center/call-segregation/call-segregation.component';
import { UserPermissionsComponent } from './user-permissions/user-permissions.component';
import { StaffListComponent } from './staff/staff-list/staff-list.component';
import { StaffAddEditComponent } from './staff/staff-add-edit/staff-add-edit.component';
import { MeetingScheduleComponent } from './meeting/meeting-schedule/meeting-schedule.component';
import { CallsPerDayComponent } from './calls-per-day/calls-per-day.component';
import { EmployeePerformanceComponent } from './employee-performance/employee-performance.component';
import { PermissionGuard } from './guards/permission.guard';
import { DesignationComponent } from './designation/designation.component';
import { VenderComponent } from './vender/vender.component';
import { VenderlistComponent } from './vender/venderlist/venderlist.component';
import { VendertoserviceComponent } from './vendertoservice/vendertoservice.component';
import { VendertoservicelistComponent } from './vendertoservice/vendertoservicelist/vendertoservicelist.component';
import { PropertyregisterComponent } from './propertyregister/propertyregister.component';
import { PropertyregisterlistComponent } from './propertyregister/propertyregisterlist/propertyregisterlist.component';
import { ViewLeadLogComponent } from './lead-log/view-lead-log/view-lead-log.component';
import { ArchivedLeadsComponent } from './archivedLeads/archived-leads/archived-leads.component';
import { NotificationListComponent } from './notification/notification-list/notification-list.component';
import { NotificationAddEditComponent } from './notification/notification-add-edit/notification-add-edit.component';
import { StaffperformanceComponent } from './staffperformance/staffperformance.component';
import { RemoveAssignedCallsComponent } from './staff/remove-assigned-calls/remove-assigned-calls.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardOneComponent,
  },
  {
    component: LeadsComponent,
    path: 'lead',
    // canActivate: [PermissionGuard],
    // data: { key: 'Lead' }
  },
  {
    component: LeadAddEditComponent,
    path: 'leadadd',
    // canActivate: [PermissionGuard],
    // data: { key: 'Lead' }
  },
  {
    component: LeadForwardComponent,
    path: 'leadForward',
    // canActivate: [PermissionGuard],
    // data: { key: 'LeadForward' }
  },
  {
    component: LeadForwardAddEditComponent,
    path: 'leadForwardadd',
    // canActivate: [PermissionGuard],
    // data: { key: 'LeadForward' }
  },
  {
    component: LeadSegregationComponent,
    path: 'leadSegregation',
    // canActivate: [PermissionGuard],
    // data: { key: 'LeadSegregation' }
  },
  {
    component: MakeCallsComponent,
    path: 'make-call',
    // canActivate: [PermissionGuard],
    // data: { key: 'CallCenter' }
  },
  {
    component: CallListComponent,
    path: 'call-list',
    // canActivate: [PermissionGuard],
    // data: { key: 'Contactlists' }
  },
  {
    component: CallSegregationComponent,
    path: 'call-segregation',
    // canActivate: [PermissionGuard],
    // data: { key: 'CallInsight' }
  },
  {
    component: UserPermissionsComponent,
    path: 'user-permission',
    // canActivate: [PermissionGuard],
    // data: { key: 'UserPermission' }
  },
  {
    component: StaffListComponent,
    path: 'staff',
    // canActivate: [PermissionGuard],
    // data: { key: 'Staffs' }
  },
  {
    component: StaffAddEditComponent,
    path: 'staffadd',
    // canActivate: [PermissionGuard],
    // data: { key: 'Staffs' }
  },
  {
    component: MeetingScheduleComponent,
    path: 'meeting',
    // canActivate: [PermissionGuard],
    // data: { key: 'CallInsight' }
  },
  {
    component: CallsPerDayComponent,
    path: 'perday',
    // canActivate: [PermissionGuard],
    // data: { key: 'ChangeDailyAmount' }
  },
  {
    component: EmployeePerformanceComponent,
    path: 'employee-performance',
    // canActivate: [PermissionGuard],
    // data: { key: 'EmployeePerformance' }
  },
  {
    component: DesignationComponent,
    path: 'designation',
    // canActivate: [PermissionGuard],
    // data: { key: 'Designation' }
  },
  {
    component: VenderComponent,
    path: 'vender',
    // canActivate: [PermissionGuard],
    // data: { key: 'VenderRegister' }
  },
  {
    component: VenderlistComponent,
    path: 'venderlist',
    // canActivate: [PermissionGuard],
    // data: { key: 'VenderRegister' }
  },
  {
    component: VendertoserviceComponent,
    path: 'vts',
    // canActivate: [PermissionGuard],
    // data: { key: 'VenderToService' }
  },
  {
    component: VendertoservicelistComponent,
    path: 'vtslist',
    // canActivate: [PermissionGuard],
    // data: { key: 'VenderToService' }
  },
  {
    component: PropertyregisterComponent,
    path: 'propertyregister',
    // canActivate: [PermissionGuard],
    // data: { key: 'PropertyRegistration' }
  },
  {
    component: PropertyregisterlistComponent,
    path: 'propertyregisterlist',
    // canActivate: [PermissionGuard],
    // data: { key: 'PropertyRegistration' }
  },
  {
    component: ViewLeadLogComponent,
    path: 'leadLog',
    // canActivate: [PermissionGuard],
    // data: { key: 'LeadLog' }
  },
  {
    component: ArchivedLeadsComponent,
    path: 'archivedLeads',
    // canActivate: [PermissionGuard],
    // data: { key: 'ArchivedLeads' }
  },
  {
    component: NotificationListComponent,
    path: 'notificationlist',
    // canActivate: [PermissionGuard],
    // data: { key: 'Notification' }
  },
  {
    component: NotificationListComponent,
    path: 'notificationlist',
    // canActivate: [PermissionGuard],
    // data: { key: 'Notification' }
  },
  {
    component: NotificationAddEditComponent,
    path: 'notificationadd',
    // canActivate: [PermissionGuard],
    // data: { key: 'Notification' }
  },
  {
    component: StaffperformanceComponent,
    path: 'staffperformance',
    // canActivate: [PermissionGuard],
    // data: { key: 'StaffPerformance' }
  },
  {
    component: RemoveAssignedCallsComponent,
    path: 'remove-assigned-calls',
    // canActivate: [PermissionGuard],
    // data: { key: 'StaffPerformance' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardsRoutingModule { }
