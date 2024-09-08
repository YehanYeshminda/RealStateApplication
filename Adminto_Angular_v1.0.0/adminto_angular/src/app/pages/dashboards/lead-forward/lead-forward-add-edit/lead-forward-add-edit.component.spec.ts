import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadForwardAddEditComponent } from './lead-forward-add-edit.component';

describe('LeadForwardAddEditComponent', () => {
  let component: LeadForwardAddEditComponent;
  let fixture: ComponentFixture<LeadForwardAddEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LeadForwardAddEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeadForwardAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
