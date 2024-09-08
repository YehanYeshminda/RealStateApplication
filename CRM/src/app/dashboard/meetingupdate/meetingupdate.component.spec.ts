import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingupdateComponent } from './meetingupdate.component';

describe('MeetingupdateComponent', () => {
  let component: MeetingupdateComponent;
  let fixture: ComponentFixture<MeetingupdateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MeetingupdateComponent]
    });
    fixture = TestBed.createComponent(MeetingupdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
