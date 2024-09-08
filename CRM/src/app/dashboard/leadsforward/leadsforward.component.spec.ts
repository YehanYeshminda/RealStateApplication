import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadsforwardComponent } from './leadsforward.component';

describe('LeadsforwardComponent', () => {
  let component: LeadsforwardComponent;
  let fixture: ComponentFixture<LeadsforwardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LeadsforwardComponent]
    });
    fixture = TestBed.createComponent(LeadsforwardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
