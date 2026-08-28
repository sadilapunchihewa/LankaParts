import { BatteryCharging, CircleDot, Cog, Fan, Gauge, LampDesk, Settings, SlidersHorizontal, Sparkles, Wrench } from 'lucide-react'

export const categories = [
  { name: 'Engine Parts', count: 1240, icon: Cog }, { name: 'Brake Parts', count: 820, icon: CircleDot }, { name: 'Suspension', count: 645, icon: SlidersHorizontal }, { name: 'Electrical', count: 910, icon: BatteryCharging }, { name: 'Filters', count: 430, icon: Sparkles }, { name: 'Transmission', count: 560, icon: Settings }, { name: 'Cooling System', count: 375, icon: Fan }, { name: 'Body Parts', count: 710, icon: Wrench }, { name: 'Lighting', count: 520, icon: LampDesk }, { name: 'Tyres & Wheels', count: 685, icon: Gauge },
]

const shared = { category: 'Vehicle Parts', sellerRating: 4.9, sellerLocation: 'Colombo', description: 'A dependable, quality-tested replacement part supplied by a verified LankaParts seller. Product specifications and vehicle fitment have been reviewed to help you order with confidence.', compatibilities: ['Toyota Aqua 2012–2021', 'Toyota Prius 2010–2015', 'Toyota Vitz 2011–2018'], reviews: 86, createdAt: 8, popularity: 98 }

export const products = [
  { ...shared, id: 1, name: 'Toyota Genuine Front Brake Pads', brand: 'Toyota', vehicle: 'Toyota Aqua 2012–2021', condition: 'New', price: 18500, seller: 'AutoZone Lanka', rating: 4.9, stock: 12, type: 'brake', category: 'Brake Parts', oem: '04465-52320', createdAt: 2, popularity: 100 },
  { ...shared, id: 2, name: 'NGK Iridium Spark Plug Set', brand: 'NGK', vehicle: 'Honda Fit GP1 / GP5', condition: 'New', price: 12900, seller: 'Nippon Auto Parts', rating: 4.8, stock: 8, type: 'spark', category: 'Engine Parts', oem: 'DILZKAR7C11S', createdAt: 5, popularity: 92 },
  { ...shared, id: 3, name: 'Front Shock Absorber Pair', brand: 'KYB', vehicle: 'Suzuki Wagon R 2014–2020', condition: 'New', price: 36500, seller: 'DriveLine Motors', rating: 4.7, stock: 5, type: 'shock', category: 'Suspension', oem: '3340068', createdAt: 1, popularity: 87 },
  { ...shared, id: 4, name: 'LED Headlight Bulb Kit H4', brand: 'Osram', vehicle: 'Universal 12V Vehicles', condition: 'New', price: 9750, seller: 'Bright Auto Hub', rating: 4.9, stock: 20, type: 'light', category: 'Lighting', oem: 'H4-LED-12V', createdAt: 3, popularity: 95 },
  { ...shared, id: 5, name: 'Hybrid Battery Cooling Fan', brand: 'Denso', vehicle: 'Toyota Aqua / Prius C', condition: 'Reconditioned', price: 28500, seller: 'Hybrid Care Lanka', rating: 4.6, stock: 4, type: 'fan', category: 'Cooling System', oem: 'G9230-52010', createdAt: 9, popularity: 76 },
  { ...shared, id: 6, name: 'Automatic Transmission Filter', brand: 'Aisin', vehicle: 'Toyota Axio 2007–2012', condition: 'New', price: 14200, seller: 'GearBox Centre', rating: 4.7, stock: 15, type: 'filter', category: 'Transmission', oem: '35330-0W021', createdAt: 4, popularity: 82 },
  { ...shared, id: 7, name: 'Original Side Mirror Assembly', brand: 'Nissan', vehicle: 'Nissan Dayz 2014–2018', condition: 'Used', price: 22000, seller: 'Japan Auto Mart', rating: 4.5, stock: 2, type: 'mirror', category: 'Body Parts', oem: '96301-6A00A', createdAt: 12, popularity: 70 },
  { ...shared, id: 8, name: 'Maintenance Oil Filter', brand: 'Bosch', vehicle: 'Mitsubishi Lancer / CS', condition: 'New', price: 4800, seller: 'MotorPro Supplies', rating: 4.8, stock: 34, type: 'filter', category: 'Filters', oem: '0986AF1045', createdAt: 6, popularity: 90 },
]

export const brands = ['Toyota', 'Honda', 'Nissan', 'Suzuki', 'Mitsubishi', 'Mazda']

export const orders = [
  { id: 'LP-260821-1042', date: '21 Aug 2026', total: 28250, items: 2, status: 'Shipped' }, { id: 'LP-260814-0987', date: '14 Aug 2026', total: 18500, items: 1, status: 'Delivered' }, { id: 'LP-260807-0914', date: '07 Aug 2026', total: 41400, items: 3, status: 'Processing' }, { id: 'LP-260725-0841', date: '25 Jul 2026', total: 9750, items: 1, status: 'Cancelled' },
]
