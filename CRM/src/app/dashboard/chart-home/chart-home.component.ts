import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { ChartHttpService } from './service/chart-http.service';
import { CalendarOptions, DateSelectArg, EventApi, EventClickArg } from '@fullcalendar/core';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import { CalenderData, DashboaordLeadCount } from './models/chart';
import { formatDate } from '@angular/common';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/baseResponse';
import { NotificationHttpService } from './service/notification-http.service';

@Component({
  selector: 'app-chart-home',
  templateUrl: './chart-home.component.html',
  styleUrls: ['./chart-home.component.scss']
})
export class ChartHomeComponent implements OnInit, AfterViewInit {
  @ViewChild('lineChart') lineChartCanvas!: ElementRef;
  @ViewChild('barChart') barChartCanvas!: ElementRef;
  @ViewChild('pieChart') pieChartCanvas!: ElementRef;
  chartLabels: string[] = [];
  chartData: number[] = [];
  openLeads$: Observable<Root<DashboaordLeadCount>> = of();
  notificationCount = 0;

  lineChartData = {
    labels: [] as string[],
    datasets: [
      {
        label: 'Leads',
        borderColor: '#6499E9',
        borderWidth: 1,
        data: [] as number[],
        fill: false,
      },
    ],
  };

  barChart!: Chart;
  lineChart!: Chart;
  pieChart!: Chart<'pie', number[], string>;

  calendarOptions: CalendarOptions = {
    plugins: [
      interactionPlugin,
      dayGridPlugin,
      timeGridPlugin,
      listPlugin,
    ],
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
    },
    initialView: 'timeGridWeek',
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    // select: this.handleDateSelect.bind(this),
    // eventClick: this.handleEventClick.bind(this),
    // eventsSet: this.handleEvents.bind(this),
    // eventAdd: this.handleEventAdd.bind(this),
    // eventChange: this.handleEventChange.bind(this),
    // eventRemove
  };

  currentEvents: EventApi[] = [];

  constructor(
    private chartHttpService: ChartHttpService,
    private changeDetector: ChangeDetectorRef,
    private notificationService: NotificationHttpService
  ) {
  }

  ngOnInit(): void {
    // this.notificationService.hubConnection.on('RefreshNotifications', (count: number) => {
    //   this.notificationCount = count;
    // });

    // this.notificationService.hubConnection.on('ReceiveSnoozeTime', (count: any) => {
    //   console.log(count);
    // });

    this.openLeads$ = this.chartHttpService.getCountForDashboard();

    this.chartHttpService.getCalenderData().subscribe({
      next: response => {
        if (response.isSuccess) {
          this.calendarOptions.events = response.result.map((item: CalenderData) => ({
            id: item.id,
            title: item.title,
            start: formatDate(item.startDate, 'yyyy-MM-ddTHH:mm:ss', 'en-US'),
          }));
        }
      }
    });

    this.loadNotifications();
  }

  ngAfterViewInit(): void {
    this.loadChartData();
    this.loadBarChart();
    this.loadPieChart();
  }

  loadChartData() {
    this.chartHttpService.getLeadLineChartData().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.lineChartData.labels = response.result.labels;
          this.lineChartData.datasets[0].data = response.result.datasets;
          this.loadLineChart();
        }
      },
    });
  }

  loadLineChart() {
    const ctx = this.lineChartCanvas.nativeElement.getContext('2d');
    const lineChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
    };

    this.lineChart = new Chart(ctx, {
      type: 'line',
      data: this.lineChartData,
      options: lineChartOptions,
    });
  }

  loadBarChart() {
    const ctx = this.barChartCanvas.nativeElement.getContext('2d');
    const barChartCanvas = {
      responsive: true,
      maintainAspectRatio: false
    };

    this.lineChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [
          {
            label: 'Weekly Overview',
            data: [65, 59, 80, 81, 56, 87, 21],
            backgroundColor: 'rgba(75, 192, 192, 0.2)',
            borderColor: '#6499E9',
            borderWidth: 1,
          },
        ],
      },
      options: barChartCanvas,
    });
  }

  loadPieChart() {
    const ctx = this.pieChartCanvas.nativeElement.getContext('2d');
    const pieChartCanvas = {
      responsive: true,
      maintainAspectRatio: false
    };
  
    this.pieChart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['September', 'October', 'November', 'December'],
        datasets: [
          {
            label: 'Monthly Data',
            data: [120, 66, 22, 12],
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(255, 206, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)',
              'rgba(153, 102, 255, 0.2)',
              'rgba(255, 159, 64, 0.2)',
              'rgba(50, 205, 50, 0.2)'
            ],
            borderColor: [
              'rgba(255, 99, 132, 1)',
              'rgba(54, 162, 235, 1)',
              'rgba(255, 206, 86, 1)',
              'rgba(75, 192, 192, 1)',
              'rgba(153, 102, 255, 1)',
              'rgba(255, 159, 64, 1)',
              'rgba(50, 205, 50, 1)'
            ],
            borderWidth: 1,
          },
        ],
      },
      options: pieChartCanvas,
    });
  }
  

  handleEventChange(arg: any) {
    console.log('Event changed:', arg);
    const changedEvent = arg.event;
    const propertyName = arg.field;
    const newValue = arg.newValue;

    console.log(`Event property "${propertyName}" changed to "${newValue}"`);
  }

  handleEventAdd(arg: any) {
    const addedEvent = arg.event;
    const startTime = addedEvent.start.toISOString();
    const endTime = addedEvent.end ? addedEvent.end.toISOString() : null;
    console.log('Start Time:', startTime);
    console.log('End Time:', endTime);
  }

  loadNotifications() {
    this.notificationService.getNotifications().subscribe({
      next: response => {
        if (response.isSuccess) {
          this.notificationCount = response.result
        }
      }
    })
  }

  handleWeekendsToggle() {
    const { calendarOptions } = this;
    calendarOptions.weekends = !calendarOptions.weekends;
  }

  handleDateSelect(selectInfo: DateSelectArg) {
    const title = prompt('Please enter a new title for your event');
    const calendarApi = selectInfo.view.calendar;

    calendarApi.unselect();

    if (title) {
      calendarApi.addEvent({
        id: '4',
        title,
        start: selectInfo.startStr,
        end: selectInfo.endStr,
        allDay: selectInfo.allDay
      });
    }
  }

  handleEventClick(clickInfo: EventClickArg) {
    if (confirm(`Are you sure you want to delete the event '${clickInfo.event.title}'`)) {
      clickInfo.event.remove();
    }
  }

  handleEvents(events: EventApi[]) {
    this.currentEvents = events;
    console.log(events)
    this.changeDetector.detectChanges();
  }
}
