import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadForwardComponent } from './lead-forward.component';

describe('LeadForwardComponent', () => {
  let component: LeadForwardComponent;
  let fixture: ComponentFixture<LeadForwardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LeadForwardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeadForwardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
