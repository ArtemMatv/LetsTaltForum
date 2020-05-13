import { Injectable, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TopicService } from './topic-service';
import { UserService } from './user-service';
import { CurrentUserService } from './currentUser-service';

@Injectable({ providedIn: 'root' })
export class PostService {
    private posts: IPost[]
    private user: IUser

    constructor(private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string,
        private topicService: TopicService,
        private userService: UserService,
        private currentUser: CurrentUserService) {
        this.http.get<IPost[]>('api/posts').subscribe((data: IPost[]) =>
            this.posts = data,
            error => console.error(error));

        this.user = this.currentUser.get()
    }

    getTotal(): number {
        return this.posts.length;
    }

    getPost(id: number): IPost {
        return this.posts.find(p => p.id == id);
    }

    getPosts(page: number, limit: number) {
        return this.posts.slice((page - 1) * limit, page * limit)
    }

    createPost(
        topicId: number,
        title: string,
        message: string,
        userId: number,
        date: string
    ): void {
        this.http.post('api/posts',
            { topicId, title, message, userId },
            { headers: { jwt: this.user.token } })
            .subscribe(
                res => {
                    console.log(res)

                    let post = {
                        id: <number>res,
                        topicId: topicId,
                        userUserName: this.user.userName,
                        title: title,
                        message: message,
                        dateCreated: date,
                        comments: []
                    }

                    this.userService.addPost(post)
                    this.topicService.addPost(post)
                    this.currentUser.addPost(post)

                    this.posts.push(post)
                },
                err => console.error(err)
            )
    }

    deletePost(id: number): void {
        this.http.delete('api/posts/' + id,
            {
                headers: { jwt: this.user.token }
            })
            .subscribe(res => {

                let post = this.posts.find(p => p.id == id)

                this.userService.removePost(post)
                this.topicService.removePost(post)
                this.currentUser.removePost(post)

                this.posts = this.posts.filter(p => p.id != id)
            }, err => console.error(err))
    }

    addComment(comment: IComment): void {
        this.posts.find(p => p.id == comment.postId).comments.push(comment)
    }

    removeComment(comment: IComment) {
        let post = this.posts.find(p => p.id == comment.postId)
        post.comments = post.comments.filter(c => c.id != comment.id)
    }
}
