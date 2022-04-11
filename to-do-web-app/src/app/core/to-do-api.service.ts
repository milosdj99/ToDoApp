import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { environment } from "src/environments/environment";
import { ToDoList } from "../models/to-do-list.model";
import { ToDoItem } from "../models/to-do-item.model";
import { ToDoSharedList } from "../models/to-do-share-list.model";



@Injectable({ providedIn: 'root' })
export class ToDoApiService {

    baseUrl = `${environment.apiUrl}to-do-lists`;

    constructor(private http: HttpClient) {
    }


    getToDoLists() {
        return this.http.get<Array<ToDoList>>(this.baseUrl);
    }

    getToDoListById(id: string){
        return this.http.get<ToDoList>(`${this.baseUrl}/lists/${id}`);
    }

    deleteToDoList(id: string){
        return this.http.delete(`${this.baseUrl}/${id}`);
    }

    deleteItem(listId: string, itemId: string){
        return this.http.delete(`${this.baseUrl}/${listId}/to-do-items/${itemId}`);
    }

    modifyItem(listId: string, item: ToDoItem){
        return this.http.put(`${this.baseUrl}/${listId}/to-do-items`, item);
    }

    modifyList(listId: string, list: ToDoList){
        return this.http.put(`${this.baseUrl}/${listId}`, list);
    }

    addItem(listId: string, item: ToDoItem){
        return this.http.post<ToDoItem>(`${this.baseUrl}/${listId}/add-item`, item);
    }

    addList(list: ToDoList){
        return this.http.post<ToDoList>(`${this.baseUrl}`, list);
    }

    modifyItemPosition(listId: string, itemId: string, newPosition: number){
        return this.http.put(`${this.baseUrl}/${listId}/item-position/${itemId}/${newPosition}`, null);
    }

    modifyListPosition(listId: string, newPosition: number){
        return this.http.put(`${this.baseUrl}/${listId}/position/${newPosition}`, null);
    }

    getListsBySearch(criteria: string){
        return this.http.get<Array<ToDoList>>(`${this.baseUrl}/search/${criteria}`);
    }

    addSharedToDoList(id: string){
        return this.http.post<string>(`${this.baseUrl}/share/${id}`, id);
    }

    getSharedToDoList(id: string){
        return this.http.get<ToDoList>(`${this.baseUrl}/share/${id}`);
    }
    
}