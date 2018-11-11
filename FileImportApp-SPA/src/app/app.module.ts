import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ImportFileComponent } from './import-file/import-file.component';
import { ImportService } from './import-file/import-file.service';
import {  MatProgressBarModule,
          MatFormFieldModule,
          MatTableModule,
          MatInputModule,
          MatSortModule,
          MatPaginatorModule, MatCardModule, MatProgressSpinnerModule } from '@angular/material';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { ViewFileComponent } from './view-file/view-file.component';
import { StoreService } from './view-file/Store.service';
import { AppConfig } from './app.config';


export function initializeApp(appConfig: AppConfig) {
  return () => appConfig.load();
}

@NgModule({
  declarations: [
    AppComponent,
    ImportFileComponent,
    ViewFileComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    MatProgressBarModule,
    MatFormFieldModule,
    MatTableModule,
    MatInputModule,
    MatSortModule,
    MatCardModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    BrowserAnimationsModule
  ],
  providers: [ImportService,
              StoreService,
              AppConfig,
              { provide: APP_INITIALIZER,
                useFactory: initializeApp,
                deps: [AppConfig], multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
