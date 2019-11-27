using RestAPI.DataAccess;
using RestAPI.DataAccess.Repository;
using RestAPI.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace RestAPI.Controllers
{
    public class OrdersController : ApiController
    {

        #region FieldsAndProperties 

        IOrdersRepository _ordersRepository = new OrdersRepository();

        #endregion

        #region Actions

        #region Get 

        /// <summary>
        /// Get All Orders
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            try
            {
                var orders = _ordersRepository.GetAll(x => x.order_id < 11);
                if (orders != null && orders.Any())
                {
                    return Content(HttpStatusCode.OK, orders.ToList());
                }
                else
                {
                    var fault = new FaultModel()
                    {
                        Code = "E2004",
                        Details = "Order Controller - Get Operation",
                        Message = " No OrdersSaved On Db."
                    };
                    return Content(HttpStatusCode.NoContent, fault);
                }
                //throw new Exception();
            }
            catch (Exception ex)
            {
                var name = ex.GetType().Name;
                var value = ConfigurationManager.AppSettings["map"];
                if (value == "1")
                {
                    return Content(HttpStatusCode.InternalServerError,
                    new FaultModel()
                    {
                        Code = "E5000",
                        Details = "Catch - InternalServerError",
                        Message = ex.Message
                    });
                }
                else if (value == "2")
                {
                    return Content(HttpStatusCode.NotFound,
                    new FaultModel()
                    {
                        Code = "E4004",
                        Details = "Catch - NotFound",
                        Message = ex.Message
                    });
                }
                else if (value == "3")
                {
                    return Content(HttpStatusCode.BadRequest,
                    new FaultModel()
                    {
                        Code = "E4000",
                        Details = "Catch - NoContent",
                        Message = ex.Message
                    });
                }
                else
                {
                    return Content(HttpStatusCode.Conflict,
                    new FaultModel()
                    {
                        Code = "E4009",
                        Details = "Catch - Conflict",
                        Message = ex.Message
                    });
                }
            }
        }

        /// <summary>
        /// Get Order By Order ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Content(HttpStatusCode.BadRequest,
                    new FaultModel()
                    {
                        Code = "E4000",
                        Details = "Order Controller - Get Operation",
                        Message = " Resouce received empty  parameters."
                    });
                }
                var dbOrder = _ordersRepository.GetSingle(x => x.order_id == id);
                if (dbOrder == null)
                {
                    return Content(HttpStatusCode.NoContent,
                    new FaultModel()
                    {
                        Code = "E2004",
                        Details = "Order Controller - Get Operation",
                        Message = " No Order With Id Saved On Db."
                    });
                }
                return Content(HttpStatusCode.OK, dbOrder);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new FaultModel()
                    {
                        Code = "E5000",
                        Details = "Order Controller - Get Operation",
                        Message = ex.Message
                    });
            }
        }

        #endregion

        #region Post

        /// <summary>
        /// Post Order to db 
        /// </summary>
        /// <param name="order"></param>
        [HttpPost]
        public IHttpActionResult Post(Order order)
        {
            try
            {
                if (order == null)
                {
                    return Content(HttpStatusCode.BadRequest,
                       new FaultModel()
                       {
                           Code = "1002",
                           Details = "Order Controller - POST Operation",
                           Message = " Resouce received empty  parameters."
                       });
                }
                else if (order.staff_id <= 1)
                {
                    return Content(HttpStatusCode.BadRequest,
                       new FaultModel()
                       {
                           Code = "E4000",
                           Details = "Order Controller - POST Operation",
                           Message = " Resouce received Not Completed parameters."
                       });
                }
                var result = _ordersRepository.Add(order);
                _ordersRepository.SaveChanges();
                return Content(HttpStatusCode.OK, new SucessModel
                {
                    Code = "E2000",
                    Details = "Order Controller - POST Operation",
                    Message = " New Order Added Successfully."
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new FaultModel()
                    {
                        Code = "E5000",
                        Details = "Order Controller - Get Operation",
                        Message = ex.Message
                    });
            }
        }

        #endregion

        #region Update 

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut]
        public IHttpActionResult Put(Order order)
        {
            try
            {
                //if (order == null)
                //{
                //    return Content(HttpStatusCode.BadRequest,
                //       new FaultModel()
                //       {
                //           Code = "E4000",
                //           Details = "Order Controller - Update Operation",
                //           Message = " Resouce received empty  parameters."
                //       });
                //}
                //else if (order.staff_id <= 1)
                //{
                //    return Content(HttpStatusCode.BadRequest, new FaultModel()
                //    {
                //        Code = "E4000",
                //        Details = "Order Controller - Update Operation",
                //        Message = " Data Not True."
                //    });
                //}
                //var result = _ordersRepository.GetSingle(x => x.order_id == order.order_id);
                //if (result == null)
                //{
                //    return Content(HttpStatusCode.NoContent,
                //    new FaultModel()
                //    {
                //        Code = "E2004",
                //        Details = "Order Controller - Update Operation",
                //        Message = " No Order With Id Saved On Db."
                //    });
                //}
                //result.order_date = DateTime.Now;
                //result.customer_id = order.customer_id;
                //_ordersRepository.SaveChanges();
                //return Content(HttpStatusCode.OK,
                //    new SucessModel
                //    {
                //        Code = "E2000",
                //        Details = "Order Controller - Update Operation",
                //        Message = " Success Path."
                //    });
                throw new Exception();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new FaultModel()
                    {
                        Code = "E4000",
                        Details = "Order Controller - Get Operation",
                        Message = ex.Message
                    });
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="id"></param>
        public IHttpActionResult Delete(int id)
        {
            try
            {
                //if (id <= 0)
                //{
                //    return Content(HttpStatusCode.BadRequest,
                //       new FaultModel()
                //       {
                //           Code = "E4000",
                //           Details = "Order Controller - Delete Operation",
                //           Message = " Resouce received empty  parameters."
                //       });
                //}
                //var result = _ordersRepository.GetSingle(x => x.order_id == id);
                //if (result == null)
                //{
                //    return Content(HttpStatusCode.NoContent,
                //    new FaultModel()
                //    {
                //        Code = "E2004",
                //        Details = "Order Controller - Delete Operation",
                //        Message = " No Order With Id Saved On Db."
                //    });
                //}
                //_ordersRepository.SaveChanges();
                //return Content(HttpStatusCode.OK, new SucessModel
                //{
                //    Code = "E2000",
                //    Details = "Order Controller - Delete Operation",
                //    Message = " Order Deleted Successfully"
                //});
                throw new Exception();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new FaultModel()
                    {
                        Code = "E5000",
                        Details = "Order Controller - Get Operation",
                        Message = ex.Message
                    });
            }
        }

        #endregion

        #endregion

    }
}
