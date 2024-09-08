import { of } from 'rxjs';
import { DashboardOneComponent } from './dashboard-one.component';
import { EmailHttpService } from 'src/app/apps/email/inbox/services/email-http.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EventService } from 'src/app/core/service/event.service';
import { FormBuilder } from '@angular/forms';

describe('DashboardOneComponent', () => {
  let component: DashboardOneComponent;
  let chartDataHttpServiceMock: any;

  describe('DashboardOneComponent', () => {
    let component: DashboardOneComponent;
    let chartDataHttpServiceMock: any;
    let eventServiceMock: EventService;
    let emailHttpServiceMock: EmailHttpService;
    let modalServiceMock: NgbModal;
    let fbMock: FormBuilder;

    beforeEach(() => {
      chartDataHttpServiceMock = jasmine.createSpyObj('ChartDataHttpService', ['getBarChartMultiData']);
      eventServiceMock = jasmine.createSpyObj('EventService', ['someMethod']);
      emailHttpServiceMock = jasmine.createSpyObj('EmailHttpService', ['someMethod']);
      modalServiceMock = jasmine.createSpyObj('ModalService', ['someMethod']);
      fbMock = jasmine.createSpyObj('FormBuilder', ['someMethod']);

      component = new DashboardOneComponent(
        eventServiceMock,
        chartDataHttpServiceMock,
        emailHttpServiceMock,
        modalServiceMock,
        fbMock
      );
    });
  });

  it('should set barChartOptions2 when getBarChartMultiData returns success', () => {
    // Arrange
    const response = {
      isSuccess: true,
      result: {
        EightAM: {
          LessThanMinute: 1,
          OneToFiveMinutes: 2
        },
        NineAM: {
          LessThanMinute: 3,
          OneToFiveMinutes: 4
        },
        TenAM: {
          LessThanMinute: 5,
          OneToFiveMinutes: 6
        },
        ElevenAM: {
          LessThanMinute: 7,
          OneToFiveMinutes: 8
        },
        TwelvePM: {
          LessThanMinute: 9,
          OneToFiveMinutes: 10
        },
        OnePM: {
          LessThanMinute: 11,
          OneToFiveMinutes: 12
        },
        TwoPM: {
          LessThanMinute: 13,
          OneToFiveMinutes: 14
        },
        ThreePM: {
          LessThanMinute: 15,
          OneToFiveMinutes: 16
        },
        FourPM: {
          LessThanMinute: 17,
          OneToFiveMinutes: 18
        },
        FivePM: {
          LessThanMinute: 19,
          OneToFiveMinutes: 20
        },
        sixPM: {
          LessThanMinute: 21,
          OneToFiveMinutes: 22
        }
      }
    };
    chartDataHttpServiceMock.getBarChartMultiData.and.returnValue(of(response));

    // Act
    component.initBarChart();

    // Assert
    expect(component.barChartOptions2).toBeDefined();
    expect(component.barChartOptions2.series).not.toBeUndefined(); // add null check
    expect(component.barChartOptions2.series!.length).toBe(2);
    expect((component.barChartOptions2.series![0] as any).name).toBe('Less Than a Minute');
    expect((component.barChartOptions2.series![0] as any).data?.length).toBe(11);
    expect((component.barChartOptions2.series![0] as any).data?.[0]).toBe(response.result.EightAM.LessThanMinute);
    expect((component.barChartOptions2.series![1] as any).name).toBe('More than 1 Minute');
    expect((component.barChartOptions2.series![1] as any).data?.length).toBe(11);
    expect((component.barChartOptions2.series![1] as any).data?.[0]).toBe(response.result.EightAM.OneToFiveMinutes);
    expect(component.barChartOptions2.xaxis?.categories?.length).toBe(11); // add null check
    expect(component.barChartOptions2.xaxis?.categories?.[0]).toBe('8AM'); // add null check
  });
});