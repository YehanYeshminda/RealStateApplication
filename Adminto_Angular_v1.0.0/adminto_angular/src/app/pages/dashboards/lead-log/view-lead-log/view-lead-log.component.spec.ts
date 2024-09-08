import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewLeadLogComponent } from './view-lead-log.component';

describe('ViewLeadLogComponent', () => {
  let component: ViewLeadLogComponent;
  let fixture: ComponentFixture<ViewLeadLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewLeadLogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewLeadLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
