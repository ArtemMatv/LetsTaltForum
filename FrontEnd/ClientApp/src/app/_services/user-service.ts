import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class UserService {
    private users: IUser[]

    constructor(private http: HttpClient) {
        http.get<IUser[]>('api/users').subscribe(result => {
            this.users = <IUser[]>result;
        }, error => console.error(error));
    }

    getTotal(): number {
        return this.users.length;
    }

    getUser(username: string): IUser {
        return this.users.find(u => u.userName == username);
    }

    getUsers(page: number, limit: number) {
        return this.users.slice((page - 1) * limit, page * limit)
    }

    addPost(post: IPost): void {
        this.users.find(u => u.userName == post.userUserName).posts.push(post)
    }

    removePost(post: IPost): void {
        let user = this.users.find(u => u.userName == post.userUserName)
        user.posts = user.posts.filter(p => p.id != post.id)
    }

    addComment(comment: IComment): void {
        this.users.find(u => u.userName == comment.userUserName).comments.push(comment)
    }

    removeComment(comment: IComment): void {
        let user = this.users.find(u => u.userName == comment.userUserName)
        user.comments = user.comments.filter(c => c.id != comment.id)
    }
}
