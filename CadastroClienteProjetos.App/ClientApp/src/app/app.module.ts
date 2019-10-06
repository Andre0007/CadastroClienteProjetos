import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';
import { AppComponent } from './app.component';
import { ClienteComponent } from '../cliente/cliente-component';
import { ClienteService } from '../service/cliente.service';
import { NgxMaskModule } from 'ngx-mask';
import { NgxPaginationModule } from 'ngx-pagination';

export const options: Partial<NgxMaskModule> | (() => Partial<NgxMaskModule>) = {};

@NgModule({
  declarations: [
    AppComponent,
    ClienteComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SlimLoadingBarModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxPaginationModule,
    NgxMaskModule.forRoot(options)
  ],
  providers: [ClienteService],
  bootstrap: [AppComponent]
})
export class AppModule { }
