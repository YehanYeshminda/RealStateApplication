import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { HomeComponent } from './home/home.component';
import { LeadsComponent } from './leads/leads.component';
import { StaffComponent } from './staff/staff.component';
import { CallcenterComponent } from './callcenter/callcenter.component';
import { LeadslistComponent } from './leads/leadslist/leadslist.component';
import { StafflistComponent } from './staff/stafflist/stafflist.component';
import { CallcenterlistComponent } from './callcenter/callcenterlist/callcenterlist.component';
import { LeadsforwardlistComponent } from './leadsforward/leadsforwardlist/leadsforwardlist.component';
import { AdvancepaymentComponent } from './advancepayment/advancepayment.component';
import { AgreementremindersComponent } from './agreementreminders/agreementreminders.component';
import { CampaindetailsComponent } from './campaindetails/campaindetails.component';
import { IouComponent } from './iou/iou.component';
import { IoureturnComponent } from './ioureturn/ioureturn.component';
import { MeetingscheduleComponent } from './meetingschedule/meetingschedule.component';
import { MeetingupdateComponent } from './meetingupdate/meetingupdate.component';
import { PaymentscheduleComponent } from './paymentschedule/paymentschedule.component';
import { PropertyassignComponent } from './propertyassign/propertyassign.component';
import { PropertydevelopmentComponent } from './propertydevelopment/propertydevelopment.component';
import { PropertyregistrationComponent } from './propertyregistration/propertyregistration.component';
import { AdvancepaymentlistComponent } from './advancepayment/advancepaymentlist/advancepaymentlist.component';
import { AgreementreminderslistComponent } from './agreementreminders/agreementreminderslist/agreementreminderslist.component';
import { CampaindetailslistComponent } from './campaindetails/campaindetailslist/campaindetailslist.component';
import { IoulistComponent } from './iou/ioulist/ioulist.component';
import { IoureturnlistComponent } from './ioureturn/ioureturnlist/ioureturnlist.component';
import { MeetingschedulelistComponent } from './meetingschedule/meetingschedulelist/meetingschedulelist.component';
import { MeetingupdatelistComponent } from './meetingupdate/meetingupdatelist/meetingupdatelist.component';
import { PaymentschedulelistComponent } from './paymentschedule/paymentschedulelist/paymentschedulelist.component';
import { PropertyassignlistComponent } from './propertyassign/propertyassignlist/propertyassignlist.component';
import { PropertydevelopmentlistComponent } from './propertydevelopment/propertydevelopmentlist/propertydevelopmentlist.component';
import { PropertyregistrationlistComponent } from './propertyregistration/propertyregistrationlist/propertyregistrationlist.component';
import { LeadsforwardComponent } from './leadsforward/leadsforward.component';
import { CreditpaymentComponent } from './creditpayment/creditpayment.component';
import { CreditpaymentlistComponent } from './creditpayment/creditpaymentlist/creditpaymentlist.component';
import { ExpenseslistComponent } from './expenses/expenseslist/expenseslist.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { VenderregisterComponent } from './venderregister/venderregister.component';
import { VenderregisterlistComponent } from './venderregister/venderregisterlist/venderregisterlist.component';
import { VendertoserviceComponent } from './vendertoservice/vendertoservice.component';
import { VendertoservicelistComponent } from './vendertoservice/vendertoservicelist/vendertoservicelist.component';
import { CustomerComponent } from './customer/customer.component';
import { CustomerlistComponent } from './customer/customerlist/customerlist.component';
import { LeadSegregationComponent } from './lead-segregation/lead-segregation.component';
import { ChartHomeComponent } from './chart-home/chart-home.component';
import { UserPermissionComponent } from './user-permission/user-permission.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationslistComponent } from './notifications/notificationslist/notificationslist.component';
import { CallinsightComponent } from './callinsight/callinsight.component';
import { ContactlistsComponent } from './contactlists/contactlists.component';

const routes: Routes = [
	{
		path: '',
		component: DashboardComponent,
		children: [
			{ path: 'home', component: HomeComponent },
			{
				path: 'advancepayment',
				component: AdvancepaymentComponent,
			},
			{
				path: 'agreementreminders',
				component: AgreementremindersComponent
			},
			{
				path: 'callcenter',
				component: CallcenterComponent,
			},
			{
				path: 'campaindetails',
				component: CampaindetailsComponent,
			},
			{
				path: 'contactlists',
				component: ContactlistsComponent,
			},
			{
				path: 'creditpayment',
				component: CreditpaymentComponent
			},
			{
				path: 'customer',
				component: CustomerComponent
			},
			{
				path: 'expense',
				component: ExpensesComponent
			},
			{
				path: 'iou',
				component: IouComponent,
			},
			{
				path: 'ioureturn',
				component: IoureturnComponent,
			},
			{
				path: 'leads',
				component: LeadsComponent,
			},
			{
				path: 'leadsforward',
				component: LeadsforwardComponent,
			},
			{
				path: 'meetingschedule',
				component: MeetingscheduleComponent,
			},
			{
				path: 'meetingupdate',
				component: MeetingupdateComponent,
			},
			{
				path: 'notifications',
				component: NotificationsComponent,
			},
			{
				path: 'paymentschedule',
				component: PaymentscheduleComponent,
			},
			{
				path: 'paymentschedule-list',
				component: PaymentschedulelistComponent,
			},
			{
				path: 'propertyassign',
				component: PropertyassignComponent,
			},
			{
				path: 'propertydevelopment',
				component: PropertydevelopmentComponent,
			},
			{
				path: 'user-permission',
				component: UserPermissionComponent,
			},
			{
				path: 'propertyregistration',
				component: PropertyregistrationComponent,
			},
			{
				path: 'staff',
				component: StaffComponent,
			},
			{
				path: 'venderregister',
				component: VenderregisterComponent,
			},
			{
				path: 'vendertoservice',
				component: VendertoserviceComponent,
			},
			{
				path: 'user-home',
				component: ChartHomeComponent,
			},

			//----------------------------------------Lists-------------------------------------------
			{
				path: 'advancepaymentlist',
				component: AdvancepaymentlistComponent,
			},
			{
				path: 'agreementreminderslist',
				component: AgreementreminderslistComponent
			},
			{
				path: 'callcenterlist',
				component: CallcenterlistComponent,
			},
			{
				path: 'campaindetailslist',
				component: CampaindetailslistComponent,
			},
			{
				path: 'creditpaymentlist',
				component: CreditpaymentlistComponent
			},
			{
				path: 'customerlist',
				component: CustomerlistComponent
			},
			{
				path: 'expenselist',
				component: ExpenseslistComponent
			},
			{
				path: 'ioulist',
				component: IoulistComponent,
			},
			{
				path: 'ioureturnlist',
				component: IoureturnlistComponent,
			},
			{
				path: 'leadslist',
				component: LeadslistComponent,
			},
			{
				path: 'leadsforwardlist',
				component: LeadsforwardlistComponent,
			},
			{
				path: 'leadssegregation',
				component: LeadSegregationComponent,
			},
			{
				path: 'meetingschedulelist',
				component: MeetingschedulelistComponent,
			},
			{
				path: 'meetingupdatelist',
				component: MeetingupdatelistComponent,
			},
			{
				path: 'notificationslist',
				component: NotificationslistComponent,
			},
			{
				path: 'paymentschedulelist',
				component: PaymentschedulelistComponent,
			},
			{
				path: 'propertyassignlist',
				component: PropertyassignlistComponent,
			},
			{
				path: 'propertydevelopmentlist',
				component: PropertydevelopmentlistComponent,
			},
			{
				path: 'propertyregistrationlist',
				component: PropertyregistrationlistComponent,
			},
			{
				path: 'stafflist',
				component: StafflistComponent,
			},
			{
				path: 'venderregisterlist',
				component: VenderregisterlistComponent,
			},
			{
				path: 'vendertoservicelist',
				component: VendertoservicelistComponent,
			},
			{
				path: 'callinsight',
				component: CallinsightComponent,
			},
		],
	},

];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class DashboardRoutingModule { }
