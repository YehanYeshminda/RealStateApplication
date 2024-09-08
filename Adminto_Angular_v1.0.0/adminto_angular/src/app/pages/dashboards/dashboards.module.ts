import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgbDatepickerModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbProgressbarModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgApexchartsModule } from 'ng-apexcharts';
import { WidgetModule } from 'src/app/shared/widget/widget.module';
import { DashboardOneComponent } from './dashboard-one/dashboard-one.component';
import { InboxComponent } from './dashboard-one/inbox/inbox.component';
import { ProjectsComponent } from './dashboard-one/projects/projects.component';
import { RevenueChartComponent } from './dashboard-one/revenue-chart/revenue-chart.component';
import { SalesChartComponent } from './dashboard-one/sales-chart/sales-chart.component';
import { StatisticsChartComponent } from './dashboard-one/statistics-chart/statistics-chart.component';
import { DashboardsRoutingModule } from './dashboards-routing.module';
import { LeadsComponent } from './leads/leads.component';
import { LeadAddEditComponent } from './leads/lead-add-edit/lead-add-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LeadForwardComponent } from './lead-forward/lead-forward.component';
import { LeadForwardAddEditComponent } from './lead-forward/lead-forward-add-edit/lead-forward-add-edit.component';
import { LeadSegregationComponent } from './lead-segregation/lead-segregation.component';
import { MakeCallsComponent } from './call-center/make-calls/make-calls.component';
import { CallListComponent } from './call-center/call-list/call-list.component';
import { CallSegregationComponent } from './call-center/call-segregation/call-segregation.component';
import { UserPermissionsComponent } from './user-permissions/user-permissions.component';
import { StaffListComponent } from './staff/staff-list/staff-list.component';
import { StaffAddEditComponent } from './staff/staff-add-edit/staff-add-edit.component';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { MeetingScheduleComponent } from './meeting/meeting-schedule/meeting-schedule.component';
import { CallsPerDayComponent } from './calls-per-day/calls-per-day.component';
import { EmployeePerformanceComponent } from './employee-performance/employee-performance.component';
import { DesignationComponent } from './designation/designation.component';
import { VenderlistComponent } from './vender/venderlist/venderlist.component';
import { VendertoserviceComponent } from './vendertoservice/vendertoservice.component';
import { VendertoservicelistComponent } from './vendertoservice/vendertoservicelist/vendertoservicelist.component';
import { VenderComponent } from './vender/vender.component';
import { PropertyregisterComponent } from './propertyregister/propertyregister.component';
import { PropertyregisterlistComponent } from './propertyregister/propertyregisterlist/propertyregisterlist.component';
import { ViewLeadLogComponent } from './lead-log/view-lead-log/view-lead-log.component';
import { ArchivedLeadsComponent } from './archivedLeads/archived-leads/archived-leads.component';
import { CalendarModule } from 'src/app/apps/calendar/calendar.module';
import { FullCalendarModule } from '@fullcalendar/angular';

import bootstrapPlugin from '@fullcalendar/bootstrap';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import { NotificationListComponent } from './notification/notification-list/notification-list.component';
import { NotificationAddEditComponent } from './notification/notification-add-edit/notification-add-edit.component';
import { StaffperformanceComponent } from './staffperformance/staffperformance.component';
import { RemoveAssignedCallsComponent } from './staff/remove-assigned-calls/remove-assigned-calls.component';

FullCalendarModule.registerPlugins([ // register FullCalendar plugins
  dayGridPlugin,
  interactionPlugin,
  bootstrapPlugin,
  timeGridPlugin,
  listPlugin
]);

@NgModule({
  declarations: [
    DashboardOneComponent,
    SalesChartComponent,
    StatisticsChartComponent,
    RevenueChartComponent,
    InboxComponent,
    ProjectsComponent,
    LeadsComponent,
    LeadAddEditComponent,
    LeadForwardComponent,
    LeadForwardAddEditComponent,
    LeadSegregationComponent,
    MakeCallsComponent,
    CallListComponent,
    CallSegregationComponent,
    UserPermissionsComponent,
    StaffListComponent,
    StaffAddEditComponent,
    MeetingScheduleComponent,
    CallsPerDayComponent,
    EmployeePerformanceComponent,
    DesignationComponent,
    VenderComponent,
    VenderlistComponent,
    VendertoserviceComponent,
    VendertoservicelistComponent,
    PropertyregisterComponent,
    PropertyregisterlistComponent,
    ViewLeadLogComponent,
    ArchivedLeadsComponent,
    NotificationListComponent,
    NotificationAddEditComponent,
    StaffperformanceComponent,
    RemoveAssignedCallsComponent
  ],
  imports: [
    CommonModule,
    NgApexchartsModule,
    NgbDropdownModule,
    WidgetModule,
    DashboardsRoutingModule,
    NgbPaginationModule,
    ReactiveFormsModule,
    FormsModule,
    NgbDatepickerModule,
    NgbNavModule,
    NgxDropzoneModule,
    NgbToastModule,
    NgbProgressbarModule,
    FullCalendarModule
  ]
})
export class DashboardsModule { }
