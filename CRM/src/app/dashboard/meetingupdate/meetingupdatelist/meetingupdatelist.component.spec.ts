import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingupdatelistComponent } from './meetingupdatelist.component';

describe('MeetingupdatelistComponent', () => {
  let component: MeetingupdatelistComponent;
  let fixture: ComponentFixture<MeetingupdatelistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MeetingupdatelistComponent]
    });
    fixture = TestBed.createComponent(MeetingupdatelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
