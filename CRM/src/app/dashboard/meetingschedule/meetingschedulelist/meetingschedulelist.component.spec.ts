import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingschedulelistComponent } from './meetingschedulelist.component';

describe('MeetingschedulelistComponent', () => {
  let component: MeetingschedulelistComponent;
  let fixture: ComponentFixture<MeetingschedulelistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MeetingschedulelistComponent]
    });
    fixture = TestBed.createComponent(MeetingschedulelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
