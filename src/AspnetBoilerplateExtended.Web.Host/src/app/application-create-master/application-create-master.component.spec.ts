import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationCreateMasterComponent } from './application-create-master.component';

describe('ApplicationCreateMasterComponent', () => {
  let component: ApplicationCreateMasterComponent;
  let fixture: ComponentFixture<ApplicationCreateMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationCreateMasterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationCreateMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
