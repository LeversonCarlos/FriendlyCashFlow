export declare interface IRepository<P, R> {
   Handle(value: P): Promise<R>;
}
