import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadsComponent } from './leads.component';
import { LeadsHttpService } from './service/leads-http.service';
import { of } from 'rxjs';

describe('LeadsComponent', () => {
  let component: LeadsComponent;
  let fixture: ComponentFixture<LeadsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LeadsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeadsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

describe('LeadsComponent', () => {
  let component: LeadsComponent;
  let fixture: ComponentFixture<LeadsComponent>;
  let leadsHttpService: jasmine.SpyObj<LeadsHttpService>;

  beforeEach(async () => {
    leadsHttpService = jasmine.createSpyObj('LeadsHttpService', ['filterLeadStatusAndStaff']);
    await TestBed.configureTestingModule({
      declarations: [ LeadsComponent ],
      providers: [
        { provide: LeadsHttpService, useValue: leadsHttpService }
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeadsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
});