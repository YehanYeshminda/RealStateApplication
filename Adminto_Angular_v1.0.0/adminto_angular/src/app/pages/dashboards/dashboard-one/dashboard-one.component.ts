import { AfterViewInit, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';

// constants
import { EventType } from 'src/app/core/constants/events';

// services
import { EventService } from 'src/app/core/service/event.service';

// types
import { BarChartData, ChartPieData, Message, Project } from './dashboard.model';

// data
import { PROJECTS } from './data';
import { ChartDataHttpService } from './services/chart-data-http.service';
import { ChartStatisticData, UpdateOldPassword } from './models/chartdata';
import { Root } from 'src/app/shared/models/base';
import { Observable, of, retry, timeout } from 'rxjs';
import { OutlookEmails } from 'src/app/apps/email/inbox/email.model';
import { EmailHttpService } from 'src/app/apps/email/inbox/services/email-http.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from '../shared/notifications/notification';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { ApexChartOptions } from '../../charts/apex/apex-chart.model';
import { TodayYesterdayPercentage } from './services/models/chart';
import { CalendarOptions, EventInput } from '@fullcalendar/core';
import { ExternalEvent } from 'src/app/apps/calendar/shared/event.model';
import { CalendarEventComponent } from 'src/app/apps/calendar/event/event.component';
import { CALENDAREVENTS, EXTERNALEVENTS } from 'src/app/apps/calendar/shared/data';

@Component({
  selector: 'app-dashboard-1',
  templateUrl: './dashboard-one.component.html',
  styleUrls: ['./dashboard-one.component.scss']
})
export class DashboardOneComponent implements OnInit, AfterViewInit {
  messages: Message[] = [];
  recentProjects: Project[] = [];
  leadsStatisticsData$: Observable<Root<ChartStatisticData>> = of();
  callsStatisticsData$: Observable<Root<ChartStatisticData>> = of();
  lostLeadStatisticsData$: Observable<Root<ChartStatisticData>> = of();
  callsLeftStatisticsData$: Observable<Root<ChartStatisticData>> = of();
  emailValues$: Observable<Root<OutlookEmails[]>> = of();
  loadedEmails: boolean = false;
  submitted = false;
  oldPasswordForm: FormGroup = new FormGroup({});

  calendarOptions: CalendarOptions = {};
  calendarEventsData: EventInput[] = [];
  selectedDay: any = {};
  isEditable: boolean = false;
  event: EventInput = {};
  externalEvents: ExternalEvent[] = [];

  @ViewChild('calendar')
  calendarComponent!: DashboardOneComponent;

  @ViewChild('eventModal', { static: true }) eventModal!: CalendarEventComponent;

  chartDataList$: Observable<Root<ChartPieData>> = of();
  barChartData$: Observable<Root<BarChartData>> = of();
  callsAssigned$: Observable<Root<BarChartData>> = of();
  getTodayYesterdayPerformance$: Observable<Root<TodayYesterdayPercentage>> = of();

  barChartOptions2: Partial<ApexChartOptions> = {};
  // isLoginFirst = true;

  @ViewChild('standardModal') standardModal!: TemplateRef<any>;
  get form() { return this.oldPasswordForm.controls; }

  constructor (
    private eventService: EventService, 
    private chartDataHttpService: ChartDataHttpService, 
    private emailHttpService: EmailHttpService,
    private modalService: NgbModal,
    private fb: FormBuilder
    ) { }

  ngOnInit(): void {
    this.fetchData();

    this.initCalendar();
    
    this.initBarChart();
    this.loadLeadsStatisticsData();
    this.loadCallsData();
    this.loadCallLeftData();
    // this.loadLostLeadData();
    this.loadPieChartData();
    // this.loadBarChartData();
    // this.loadCallsAssignedBarChartData();
    // this.loadTodayYesterdayPerformanceData();

    this.eventService.broadcast(EventType.CHANGE_PAGE_TITLE,
      {
        title: 'Dashboard',
        breadCrumbItems: [{ label: 'Dashboards', path: '/' }, { label: 'Dashboard', path: '/', active: true }]
      }
    );
    this._fetchData();
    this.loadAllEmails();
    // this.open();
  }

  ngAfterViewInit(): void {

    this.chartDataHttpService.checkIfFirstLogin().subscribe({
      next: response => {
        if(response.isSuccess) {
          if (response.result == "1") {
            this.initializeForm();
            this.open();
          }
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  fetchData(): void {
    this.calendarEventsData = CALENDAREVENTS;
    this.externalEvents = EXTERNALEVENTS;
  }

  initCalendar(): void {

    // full calendar config
    this.calendarOptions = {
      themeSystem: 'bootstrap',
      bootstrapFontAwesome: false,
      buttonText: {
        today: 'Today',
        month: 'Month',
        week: 'Week',
        day: 'Day',
        list: 'List',
        prev: 'Prev',
        next: 'Next'
      },
      initialView: 'timeGridWeek',
      handleWindowResize: true,
      headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
      },
      events: [...this.calendarEventsData],
      editable: true,
      droppable: true,
      selectable: true,
    }
  }

  updatePassword() {
    this.submitted = true;

    if (this.oldPasswordForm.valid) {
      const data: UpdateOldPassword = {
        authDto: GetAuthDetails(),
        password: this.oldPasswordForm.controls['password'].value,
        oldPassword: this.oldPasswordForm.controls['oldPassword'].value,
      };

      this.chartDataHttpService.passwordReset(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.modalService.dismissAll();
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  }

  cancelUpdate() {
    this.modalService.dismissAll();
    this.chartDataHttpService.passwordResetCancel().subscribe({
      next: response => {
        if (response.isSuccess) {
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  initializeForm() {
    this.oldPasswordForm = this.fb.group({
      oldPassword: ['', [Validators.required]],
      password: ['', [Validators.required]]
    })
  }
  
  _fetchData(): void {
    this.recentProjects = PROJECTS;
  }

  loadLeadsStatisticsData() {
    this.leadsStatisticsData$ = this.chartDataHttpService.getLeadChartStatisticsData().pipe(
      timeout(5000),
      retry(3),
    );
  }

  loadCallsData() {
    this.callsStatisticsData$ = this.chartDataHttpService.getCallInsigntStatisticsData().pipe(
      timeout(5000),
      retry(3),
    );
  }

  loadCallLeftData() {
    this.callsLeftStatisticsData$ = this.chartDataHttpService.getCallsLeftStatisticsData().pipe(
      timeout(5000),
      retry(3),
    );
  }

  loadLostLeadData() {
    this.lostLeadStatisticsData$ = this.chartDataHttpService.getLostLeadChartStatisticsData().pipe(
      timeout(5000),
      retry(3),
    );
  }

  loadPieChartData() {
    this.chartDataList$ = this.chartDataHttpService.getPieChartStatisticsData().pipe(
      timeout(5000),
      retry(3),
    );
  }

  loadBarChartData() {
    this.barChartData$ = this.chartDataHttpService.getBarChartStatisticsData().pipe(
      timeout(5000),
      retry(3),
    );
  }

  loadTodayYesterdayPerformanceData() {
    this.getTodayYesterdayPerformance$ = this.chartDataHttpService.getTodayYesterdayPerformance().pipe(
      timeout(5000),
      retry(3),
    );
  }

  loadCallsAssignedBarChartData() {
    this.callsAssigned$ = this.chartDataHttpService.getBarCallsAssignedChartStatisticsData().pipe(
      timeout(5000),
      retry(3)
    );
  }

  loadAllEmails() {
    this.loadedEmails = false;
    this.emailHttpService.getAllEmails().subscribe({
      next: response => {

        response.result.forEach((element, i) => {
          const message: Message = {
            id: i,
            avatar: "assets/images/users/user-1.jpg",
            sender: element.from,
            text: element.subject,
            time: ``
          };

          this.messages.push(message);

          this.loadedEmails = true;
        });
      }
    })
  }

  open(): void {
    this.modalService.open(this.standardModal, { backdrop: 'static', keyboard: true });
  }

  initBarChart(): void {
    // this.chartDataHttpService.getBarChartMultiData().subscribe({
    //   next: response => {
    //     if (response.isSuccess) {
    //       this.barChartOptions2 = {
    //         series: [
    //           {
    //             name: 'Less Than a Minute',
    //             data: [
    //               response.result.EightAM.LessThanMinute,
    //               response.result.NineAM.LessThanMinute,
    //               response.result.TenAM.LessThanMinute,
    //               response.result.ElevenAM.LessThanMinute,
    //               response.result.TwelvePM.LessThanMinute,
    //               response.result.OnePM.LessThanMinute,
    //               response.result.TwoPM.LessThanMinute,
    //               response.result.ThreePM.LessThanMinute,
    //               response.result.FourPM.LessThanMinute,
    //               response.result.FivePM.LessThanMinute,
    //               response.result.sixPM.LessThanMinute,
    //             ],
    //           },
    //           {
    //             name: 'More than 1 Minute',
    //             data: [
    //               response.result.EightAM.OneToFiveMinutes,
    //               response.result.NineAM.OneToFiveMinutes,
    //               response.result.TenAM.OneToFiveMinutes,
    //               response.result.ElevenAM.OneToFiveMinutes,
    //               response.result.TwelvePM.OneToFiveMinutes,
    //               response.result.OnePM.OneToFiveMinutes,
    //               response.result.TwoPM.OneToFiveMinutes,
    //               response.result.ThreePM.OneToFiveMinutes,
    //               response.result.FourPM.OneToFiveMinutes,
    //               response.result.FivePM.OneToFiveMinutes,
    //               response.result.sixPM.OneToFiveMinutes,
    //             ],
    //           },
    //         ],
    //         chart: {
    //           height: 380,
    //           type: 'bar',
    //           stacked: true,
    //           toolbar: {
    //             show: false,
    //           },
    //         },
    //         plotOptions: {
    //           bar: {
    //             horizontal: false,
    //           },
    //         },
    //         stroke: {
    //           show: false,
    //         },
    //         xaxis: {
    //           categories: ["8AM", "9AM", "10AM", "11AM", "12PM", "1PM", "2PM", "3PM", "4PM", "5PM", "6PM"],
    //           labels: {
    //             formatter: (val: string) => {
    //               return val + '';
    //             },
    //           },
    //         },
    //         yaxis: {
    //           title: {
    //             text: undefined,
    //           },
    //         },
    //         colors: ['#219ebc', '#ffb703', '#e63946'],
    //         tooltip: {
    //           y: {
    //             formatter: (val: number) => {
    //               return val + ' Call';
    //             },
    //           },
    //         },
    //         fill: {
    //           opacity: 1,
    //         },
    //         states: {
    //           hover: {
    //             filter: {
    //               type: 'none',
    //             },
    //           },
    //         },
    //         legend: {
    //           position: 'top',
    //           horizontalAlign: 'center',
    //         },
    //         grid: {
    //           borderColor: '#f7f7f7',
    //         },
    //       };
    //     }
    //   }
    // })
  }
}
