export const sellerProducts = [
  { id: 101, name: 'Toyota Aqua Front Brake Pads', categoryId: 2, categoryName: 'Brake Parts', brand: 'Toyota', partNumber: '04465-52320', description: 'Genuine front brake pad set for hybrid Toyota compact models.', price: 18500, stockQuantity: 12, condition: 'New', imageUrl: '', vehicleMake: 'Toyota', vehicleModel: 'Aqua', yearFrom: 2012, yearTo: 2021, approvalStatus: 'Approved', status: 'Active' },
  { id: 102, name: 'Hybrid Battery Cooling Fan', categoryId: 7, categoryName: 'Cooling System', brand: 'Denso', partNumber: 'G9230-52010', description: 'Reconditioned hybrid battery cooling fan, bench tested.', price: 28500, stockQuantity: 4, condition: 'Reconditioned', imageUrl: '', vehicleMake: 'Toyota', vehicleModel: 'Prius C', yearFrom: 2012, yearTo: 2018, approvalStatus: 'Pending', status: 'Active' },
  { id: 103, name: 'Nissan Dayz Side Mirror Assembly', categoryId: 8, categoryName: 'Body Parts', brand: 'Nissan', partNumber: '96301-6A00A', description: 'Used original left side mirror assembly.', price: 22000, stockQuantity: 0, condition: 'Used', imageUrl: '', vehicleMake: 'Nissan', vehicleModel: 'Dayz', yearFrom: 2014, yearTo: 2018, approvalStatus: 'Rejected', rejectionReason: 'Upload clearer product images and confirm OEM number.', status: 'Inactive' },
]

export const sellerOrders = [
  { orderId: 90021, orderNumber: 'LP-260828-1120', customerName: 'Ayesh Fernando', customerEmail: 'ayesh@example.com', contactPhone: '+94 77 456 8890', shippingAddress: '42 Galle Road', shippingCity: 'Colombo', status: 'Pending', sellerSubtotal: 47000, createdAt: '2026-08-28T08:15:00Z', items: [{ orderItemId: 1, sparePartId: 101, partName: 'Toyota Aqua Front Brake Pads', partNumber: '04465-52320', unitPrice: 18500, quantity: 1, lineTotal: 18500, fulfillmentStatus: 'Pending' }, { orderItemId: 2, sparePartId: 102, partName: 'Hybrid Battery Cooling Fan', partNumber: 'G9230-52010', unitPrice: 28500, quantity: 1, lineTotal: 28500, fulfillmentStatus: 'Pending' }] },
  { orderId: 90018, orderNumber: 'LP-260826-1098', customerName: 'Nimali Jayasinghe', customerEmail: 'nimali@example.com', contactPhone: '+94 71 240 5521', shippingAddress: '15 Temple Lane', shippingCity: 'Kandy', status: 'Processing', sellerSubtotal: 37000, createdAt: '2026-08-26T10:40:00Z', items: [{ orderItemId: 3, sparePartId: 101, partName: 'Toyota Aqua Front Brake Pads', partNumber: '04465-52320', unitPrice: 18500, quantity: 2, lineTotal: 37000, fulfillmentStatus: 'Processing' }] },
  { orderId: 90011, orderNumber: 'LP-260821-1042', customerName: 'Kasun Perera', customerEmail: 'kasun@example.com', contactPhone: '+94 76 101 2233', shippingAddress: '8 Lake Drive', shippingCity: 'Gampaha', status: 'Delivered', sellerSubtotal: 28500, createdAt: '2026-08-21T12:10:00Z', items: [{ orderItemId: 4, sparePartId: 102, partName: 'Hybrid Battery Cooling Fan', partNumber: 'G9230-52010', unitPrice: 28500, quantity: 1, lineTotal: 28500, fulfillmentStatus: 'Delivered' }] },
]

export const earningsHistory = [
  { month: 'August 2026', grossSales: 500000, commission: 15000, netEarnings: 485000, pendingSettlement: 125000, paidSettlement: 360000 },
  { month: 'July 2026', grossSales: 420000, commission: 12600, netEarnings: 407400, pendingSettlement: 0, paidSettlement: 407400 },
  { month: 'June 2026', grossSales: 368000, commission: 11040, netEarnings: 356960, pendingSettlement: 0, paidSettlement: 356960 },
  { month: 'May 2026', grossSales: 296000, commission: 8880, netEarnings: 287120, pendingSettlement: 0, paidSettlement: 287120 },
]

export const orderStatusFlow = ['Pending', 'Confirmed', 'Processing', 'Shipped', 'Delivered']
