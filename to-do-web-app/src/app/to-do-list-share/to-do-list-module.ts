import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { ToDoListShareComponent } from "./to-do-list-share.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MatButtonModule } from "@angular/material/button";
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatListModule} from '@angular/material/list'
import {MatCardModule} from '@angular/material/card';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { DragDropModule } from "@angular/cdk/drag-drop";
import { MatInputModule } from "@angular/material/input";
import { MatDividerModule } from '@angular/material/divider';


@NgModule({
    declarations: [
        ToDoListShareComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MatButtonModule,
        MatToolbarModule,
        MatListModule,
        MatCardModule,
        MatCheckboxModule,
        DragDropModule,
        MatInputModule,
        MatDividerModule
    ],
    exports: [
    ]
})
export class ToDoListShareModule { }