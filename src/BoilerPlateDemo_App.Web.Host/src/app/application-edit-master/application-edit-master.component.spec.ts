import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationEditMasterComponent } from './application-edit-master.component';

describe('ApplicationEditMasterComponent', () => {
  let component: ApplicationEditMasterComponent;
  let fixture: ComponentFixture<ApplicationEditMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationEditMasterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationEditMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
