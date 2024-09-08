import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MakeCallsComponent } from './make-calls.component';

describe('MakeCallsComponent', () => {
  let component: MakeCallsComponent;
  let fixture: ComponentFixture<MakeCallsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MakeCallsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MakeCallsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
